﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shiftbook.Models;

namespace Shiftbook.DAL
{
    public class UserDAL
    {
        public List<User> GetAllUsersList(DatabaseEntities de)
        {
            return de.Users.ToList();
        }

        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return de.Users.Where(x=> x.IsActive == 1).ToList();
        }
        
        public List<User> GetActiveUsersListByDep(int id, DatabaseEntities de)
        {
            return de.Users.Where(x=> x.DepartmentId == id && x.IsActive == 1).ToList();
        }

        public User GetUserById(int id, DatabaseEntities de)
        {
            return de.Users.Where(x=> x.Id == id).FirstOrDefault();
        }

        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            return de.Users.Where(x => x.Id == id && x.IsActive == 1).FirstOrDefault();
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int AddUser2(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                de.SaveChanges();

                return user.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Entry(user).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                if (user.IsActive == 0)
                {
                    List<WorkOrder> OrderList = new WorkOrderDAL().GetActiveWorkOrdersList(de).Where(x => x.UserId == user.Id).ToList();
                    foreach (WorkOrder o in OrderList)
                    {
                        o.IsActive = 0;
                        new WorkOrderDAL().UpdateWorkOrder(o, de);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(int id, DatabaseEntities de)
        {
            try
            {
                de.Users.Remove(de.Users.Where(x => x.Id == id).FirstOrDefault());
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}