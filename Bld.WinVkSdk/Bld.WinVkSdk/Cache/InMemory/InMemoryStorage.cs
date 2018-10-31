namespace Bld.WinVkSdk.Cache.InMemory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class InMemoryStorage : IApiCache
    {
        private readonly List<CacheObject<VkUser>> _users = new List<CacheObject<VkUser>>();
        private readonly object _usersLock = new object();
        private List<ulong> _friendsIds;

        public List<VkUser> GetFriends()
        {
            lock (_usersLock)
            {
                if (_friendsIds == null)
                {
                    return null;
                }

                var toRet = new List<VkUser>();
                foreach (var id in _friendsIds)
                {
                    toRet.Add(_users.First(o => o.Value.Id == id));
                }

                return toRet;
            }
        }

        public VkUser GetUserById(ulong id)
        {
            lock (_usersLock)
            {
                return _users.FirstOrDefault(o => o.Value.Id == id);
            }
        }

        public void Update(VkUser user)
        {
            lock (_usersLock)
            {
                var old = _users.FirstOrDefault(o => o.Value.Id == user.Id);
                if (old != null)
                {
                    old.Update(user);
                }
                else
                {
                    _users.Add(new CacheObject<VkUser>(user));
                }
            }
        }

        public void UpdateFriends(List<VkUser> friends)
        {
            lock (_usersLock)
            {
                if (_friendsIds == null)
                {
                    _friendsIds = new List<ulong>();
                }

                _friendsIds.Clear();
                foreach (var user in friends)
                {
                    if(user != null)
                    {
                        Update(user);
                        _friendsIds.Add(user.Id);
                    }
                }
            }
        }
        
    }
}
