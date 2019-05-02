using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Extensions;

namespace TrainSchdule.BLL.Services
{
    /// <summary>
    /// Contains methods with users processing logic.
    /// Realization of <see cref="IUsersService"/>.
    /// </summary>
    public class UsersService : IUsersService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/>.
        /// </summary>
        public UsersService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = new CurrentUserService(unitOfWork, httpContextAccessor);
        }

        #endregion

        #region Logic

        /// <summary>
        /// Loads all users with paggination, returns collection of user DTOs.
        /// </summary>
        public IEnumerable<UserDTO> GetAll(int page, int pageSize)
        {
            var users = _unitOfWork.Users.GetAll(page, pageSize);
            var userDTOs = new List<UserDTO>(users.Count());

            foreach (var user in users)
            {
                userDTOs.Add(MapUser(user));
            }

            return userDTOs;
        }

        /// <summary>
        /// Loads user by username, returns user DTO.
        /// </summary>
        public UserDetailsDTO Get(string userName)
        {
            var user = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();

            return MapUserDetails(user);
        }

        public IQueryable<User> Find(Expression<Func<User, bool>> predict)
        {
	        return _unitOfWork.Users.Find(predict).OrderBy(u=>u.Privilege).ThenBy(u=>u.RealName);
        }

        /// <summary>
        /// Loads blocked users by this user.
        /// </summary>
        public IEnumerable<UserDTO> GetBlocked(int page, int pageSize)
        {
            var blacklists = _unitOfWork.Blockings.Find(b => b.User.Id == _currentUserService.CurrentUser.Id);
            var users = new List<User>();

            foreach(var blocking in blacklists)
            {
                users.Add(blocking.BlockedUser);
            }

            var userDTOs = new List<UserDTO>(pageSize);

            foreach (var user in users)
            {
                userDTOs.Add(MapUser(user));
            }

            return userDTOs;
        }

        /// <summary>
        /// Method for searching users.
        /// </summary>
        public IEnumerable<UserDTO> Search(int page, string search, int pageSize)
        {
            var currentUser = _currentUserService.CurrentUser;
            IEnumerable<User> users;

            if (string.IsNullOrEmpty(search))
            {
                users = _unitOfWork.Users.Find(u => u.UserName != currentUser.UserName).OrderByDescending(u => u.Date).Skip(page * pageSize).Take(pageSize);
            }
            else
            {
                users = _unitOfWork.Users.Find(u =>
                    u.UserName != currentUser.UserName &&
                    (
                        u.UserName.ToLower().Contains(search.ToLower()) || (!string.IsNullOrEmpty(u.RealName) ? u.RealName.ToLower().Contains(search.ToLower()) : false)
                    )
                ).OrderBy(u => u.Date).Skip(page * pageSize).Take(pageSize);
            }

            var userDTOs = new List<UserDTO>();

            foreach(var user in users)
            {
                userDTOs.Add(MapUser(user));
            }

            return userDTOs;
        }

        /// <summary>
        /// Follows user by current user.
        /// </summary>
        public void Follow(string follow)
        {
            var currentUser = _currentUserService.CurrentUser;
            var followedUser = _unitOfWork.Users.Find(u => u.UserName == follow).FirstOrDefault();

            if (currentUser != null && followedUser != null && currentUser.UserName != follow && _unitOfWork.Followings.Find(f => f.User.Id == currentUser.Id && f.FollowedUser.Id == followedUser.Id).FirstOrDefault() == null)
            {
                _unitOfWork.Followings.Create(new Following
                {
					User = new User()
					{
						Id = currentUser.Id
					},
					FollowedUser = new User()
					{
						Id = followedUser.Id
					}
				});

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async follows user by current user.
        /// </summary>
        public async Task FollowAsync(string follow)
        {
            var currentUser = _currentUserService.CurrentUser;
            var followedUser = _unitOfWork.Users.Find(u => u.UserName == follow).FirstOrDefault();

            if (currentUser != null && followedUser != null && currentUser.UserName != follow && _unitOfWork.Followings.Find(f => f.User.Id == currentUser.Id && f.FollowedUser.Id == followedUser.Id).FirstOrDefault() == null)
            {
                await _unitOfWork.Followings.CreateAsync(new Following
                {
                    User=new User()
                    {
						Id = currentUser.Id
					},
                    FollowedUser = new User()
                    {
						Id= followedUser.Id
					}
                });

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Dismiss following on user by current user.
        /// </summary>
        public void DismissFollow(string follow)
        {
            var currentUser = _currentUserService.CurrentUser;
            var followedUser = _unitOfWork.Users.Find(u => u.UserName == follow).FirstOrDefault();
            var following = _unitOfWork.Followings.Find(f => f.User.Id == currentUser.Id && f.FollowedUser.Id == followedUser.Id).FirstOrDefault();

            if (currentUser != null && followedUser != null && currentUser.UserName != follow && following != null)
            {
                _unitOfWork.Followings.Delete(following.Id);

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async dismiss following on user by current user.
        /// </summary>
        public async Task DismissFollowAsync(string follow)
        {
            var currentUser = _currentUserService.CurrentUser;
            var followedUser = _unitOfWork.Users.Find(u => u.UserName == follow).FirstOrDefault();
            var following = _unitOfWork.Followings.Find(f => f.User.Id == currentUser.Id && f.FollowedUser.Id == followedUser.Id).FirstOrDefault();

            if (currentUser != null && followedUser != null && currentUser.UserName != follow && following != null)
            {
                await _unitOfWork.Followings.DeleteAsync(following.Id);

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Blocks user by current user.
        /// </summary>
        public void Block(string block)
        {
            var currentUser = _currentUserService.CurrentUser;
            var blockedUser = _unitOfWork.Users.Find(u => u.UserName == block).FirstOrDefault();

            if (currentUser != null && blockedUser != null && currentUser.UserName != block && _unitOfWork.Blockings.Find(b => b.User.Id == currentUser.Id && b.BlockedUser.Id == blockedUser.Id).FirstOrDefault() == null)
            {
                _unitOfWork.Blockings.Create(new BlackList
                {
					User = new User()
					{
						Id = currentUser.Id
					},
					BlockedUser = new User()
					{
						Id = blockedUser.Id
					}
				});

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async blocks user by current user.
        /// </summary>
        public async Task BlockAsync(string block)
        {
            var currentUser = _currentUserService.CurrentUser;
            var blockedUser = _unitOfWork.Users.Find(u => u.UserName == block).FirstOrDefault();

            if (currentUser != null && blockedUser != null && currentUser.UserName != block && _unitOfWork.Blockings.Find(b => b.User.Id == currentUser.Id && b.BlockedUser.Id == blockedUser.Id).FirstOrDefault() == null)
            {
                await _unitOfWork.Blockings.CreateAsync(new BlackList
                {
                    User =new User()
                    {
						Id = currentUser.Id
					},
                    BlockedUser = new User()
                    {
						Id = blockedUser.Id
					}
                });

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Dismiss blocking user by current user.
        /// </summary>
        public void DismissBlock(string block)
        {
            var currentUser = _currentUserService.CurrentUser;
            var blockedUser = _unitOfWork.Users.Find(u => u.UserName == block).FirstOrDefault();
            var blocking = _unitOfWork.Blockings.Find(b => b.User.Id == currentUser.Id && b.BlockedUser.Id == blockedUser.Id).FirstOrDefault();

            if (currentUser != null && blockedUser != null && currentUser.UserName != block && blocking != null)
            {
                _unitOfWork.Blockings.Delete(blocking.Id);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async dismiss blocking user by current user.
        /// </summary>
        public async Task DismissBlockAsync(string block)
        {
            var currentUser = _currentUserService.CurrentUser;
            var blockedUser = _unitOfWork.Users.Find(u => u.UserName == block).FirstOrDefault();
            var blocking = _unitOfWork.Blockings.Find(b => b.User.Id == currentUser.Id && b.BlockedUser.Id == blockedUser.Id).FirstOrDefault();

            if (currentUser != null && blockedUser != null && currentUser.UserName != block && blocking != null)
            {
                await _unitOfWork.Blockings.DeleteAsync(blocking.Id);
                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Reports user by current user.
        /// </summary>
        public void Report(string userName, string text)
        {
            var currentUser = _currentUserService.CurrentUser;
            var reportedUser = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();

            if (currentUser != null && reportedUser != null && currentUser.UserName != userName && _unitOfWork.UserReports.Find(b => b.User.Id == currentUser.Id && b.ReportedUser.Id == reportedUser.Id).FirstOrDefault() == null)
            {
                _unitOfWork.UserReports.Create(new UserReport
                {
                    User =new User(){Id= currentUser.Id},
                    ReportedUser=new User(){Id= reportedUser.Id},
                    Text = text
                });

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async reports user by current user.
        /// </summary>
        public async Task ReportAsync(string userName, string text)
        {
            var currentUser = _currentUserService.CurrentUser;
            var reportedUser = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();

            if (currentUser != null && reportedUser != null && currentUser.UserName != userName && _unitOfWork.UserReports.Find(b => b.User.Id == currentUser.Id && b.ReportedUser.Id == reportedUser.Id).FirstOrDefault() == null)
            {
                _unitOfWork.UserReports.Create(new UserReport
                {
                    User =new User(){Id= currentUser.Id},
                    ReportedUser=new User(){Id= reportedUser.Id},
                    Text = text
                });

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Creates user.
        /// </summary>
        public ApplicationUser Create(string userName, string email, string password,string company)
        {
            if (_unitOfWork.IdentityUsers.Find(u => u.UserName == userName || u.Email == email).FirstOrDefault() != null)
            {
                return null;
            }

            var identity = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = userName.ToUpper(),
                LockoutEnabled = true,
                TwoFactorEnabled = false,
                SecurityStamp = userName.GetHashCode().ToString()
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            identity.PasswordHash = passwordHasher.HashPassword(identity, password);
            
            _unitOfWork.IdentityUsers.Create(identity);

            var userCompany = _unitOfWork.Companies.Find(x => x.Code == company).FirstOrDefault();
            _unitOfWork.Users.Create(new User
            {
	            UserName = userName,
				Company = userCompany
            });
            _unitOfWork.Save();

            return identity;
        }

        /// <summary>
        /// Async creates user.
        /// </summary>
        public async Task<ApplicationUser> CreateAsync(string userName, string email, string password,string company)
        {
            if (_unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault() != null)
            {
                return null;
            }

            var identity = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                PhoneNumberConfirmed = false,
                EmailConfirmed = false,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = userName.ToUpper(),
                LockoutEnabled = true,
                TwoFactorEnabled = false,
                SecurityStamp = userName.GetHashCode().ToString()
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            identity.PasswordHash = passwordHasher.HashPassword(identity, password);

            await _unitOfWork.IdentityUsers.CreateAsync(identity);

            var userCompany = _unitOfWork.Companies.Find(x => x.Code == company).FirstOrDefault();
            await _unitOfWork.Users.CreateAsync(new User
            {
	            UserName = userName,
				Company = userCompany
            });
            await _unitOfWork.SaveAsync();

            return identity;
        }

        /// <summary>
        /// Edits user.
        /// </summary>
        public bool Edit(string userName, Action<User> editCallBack)
        {
            var user = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();
            if (user == null) return false;
            editCallBack.Invoke(user);
			_unitOfWork.Users.Update(user);
            return true;
		}

        /// <summary>
        /// Async edits user.
        /// </summary>
        public async Task<bool> EditAsync(string userName, Action<User> editCallBack)
        {
            var user = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();

			if(user==null)return false;
			await Task.Run(()=> editCallBack.Invoke(user));
			_unitOfWork.Users.Update(user);
			return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helps map user entity to user data transfer object.
        /// </summary>
        protected UserDTO MapUser(User user)
        {
            var currentUser = _currentUserService.CurrentUser;

            return user.ToDTO(_unitOfWork.Confirmations.Find(c => c.User.Id == user.Id).FirstOrDefault() != null,
                              _unitOfWork.Followings.Find(f => f.FollowedUser.Id == user.Id && f.User.Id == currentUser.Id).FirstOrDefault() != null,
                              _unitOfWork.Blockings.Find(b => b.BlockedUser.Id == user.Id && b.User.Id == currentUser.Id).FirstOrDefault() != null,
                              _unitOfWork.Blockings.Find(b => b.BlockedUser.Id == currentUser.Id && b.User.Id == user.Id).FirstOrDefault() != null);
        }

        /// <summary>
        /// Helps map user details entity to user details data transfer object.
        /// </summary>
        protected UserDetailsDTO MapUserDetails(User user)
        {
            var currentUser = _currentUserService.CurrentUser;

            return user.ToDetailDTO();
            
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                    _currentUserService.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~UsersService()
        {
            Dispose(false);
        }

        #endregion
    }
}