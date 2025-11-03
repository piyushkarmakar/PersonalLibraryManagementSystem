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

            public FriendService(List<Friend> friends)
            {
                this.friends = friends;
            }

            // Add a new friend
            public void Add(Friend friend)
            {
                friends.Add(friend);
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
            return friends;
        }

    }



}
