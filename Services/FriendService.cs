using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Interfaces;
using PersonalLibraryManagementSystem.Models;



namespace PersonalLibraryManagementSystem.Services
{



        public class FriendService : IManageable<Friend>
        {
            private List<Friend> friends;
            private bool useDatabase = false;
            private DatabaseService dbService;

        // File Mode
        public FriendService(List<Friend> friends)
        {
            this.friends = friends;
            this.useDatabase = false;
        }

        // SQL Mode
        public FriendService(string connectionString)
        {
            this.dbService = new DatabaseService(connectionString);
            this.useDatabase = true;
            this.friends = dbService.GetAllFriends();
        }

        // Add a new friend
        public void Add(Friend friend)
        {
            if (useDatabase)
            {
                dbService.AddFriend(friend);
                friends = dbService.GetAllFriends();
            }
            else
            {
                friends.Add(friend);
            }
        }
        // ✅ UPDATE FRIEND BY ID
        public void Update(int id, Friend updatedFriend)
        {
            Friend friend = friends.Find(f => f.Id == id);

            if (friend != null)
            {
                if (useDatabase)
                {

                    dbService.UpdateFriendById(updatedFriend);

                    friends = dbService.GetAllFriends(); // refresh
                }
                else
                {
                    friend.Name = updatedFriend.Name;
                    friend.Email = updatedFriend.Email;
                    friend.Phone = updatedFriend.Phone;
                }
            }
        }

        // ✅ DELETE FRIEND BY ID
        public void Remove(int id)
        {
            Friend friend = friends.Find(f => f.Id == id);

            if (friend != null)
            {
                if (useDatabase)
                {
                    dbService.DeleteFriendById(id);

                    friends = dbService.GetAllFriends();
                }
                else
                {
                    friends.Remove(friend);
                }
            }
        }


        // Get friend by index
        public Friend GetById(int id)
            {
                if (id >= 0 && id < friends.Count)
                {
                    return friends[id];
                }
                return null;
            }

        public Friend GetByName(string name)
        {
            foreach (Friend f in friends)
            {
                if (f.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return f;
            }
            return null;
        }

        // Search by name
        public List<Friend> Search(string name)
            {
                List<Friend> results = new List<Friend>();

                foreach (Friend f in friends)
                {
                    if (f.Name != null && f.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(f);
                    }
                }

                return results;
            }

        public List<Friend> GetAllFriends()
        {
            if (useDatabase)
                return dbService.GetAllFriends();
            return friends;
        }

    }



}
