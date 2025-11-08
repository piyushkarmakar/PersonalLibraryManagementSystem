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
        // Update friend details
        public void Update(int id, Friend updatedFriend)
            {
                // Find the friend by matching email
                Friend friend = null;
                foreach (var f in friends)
                {
                    if (f.Email == updatedFriend.Email)
                    {
                        friend = f;
                        break;
                    }
                }

                // If found, update name and phone
                if (friend != null)
                {
                    friend.Name = updatedFriend.Name;
                    friend.Phone = updatedFriend.Phone;
                }
            }

            // Remove friend by name or email
            public void Remove(int id)
            {
                // Assuming id is index in the list
                if (id >= 0 && id < friends.Count)
                    friends.RemoveAt(id);
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
