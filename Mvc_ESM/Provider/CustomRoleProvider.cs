using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
/// <summary>
/// Summary description for RoleProvider
/// </summary>
public class CustomRoleProvider : System.Web.Security.RoleProvider
{
    ProviderEntities db = new ProviderEntities();
    public override string ApplicationName
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="usernames">a list of usernames</param>
    /// <param name="roleNames">a list of roles</param>
    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
            
        foreach (var user in usernames)
        {
            foreach (var role in roleNames)
            {
                var UserID = db.Users.Single(m => m.ID == user);
                var RoleID = db.Roles.Single(m => m.ID == role);
                var g = new UsersRoles() { Users = UserID, Roles = RoleID };
                db.UsersRoles.Add(g);
                db.SaveChanges();
            }
        }
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="roleName">a role name</param>
    public override void CreateRole(string roleName)
    {
        db.Roles.Add(new Role() { ID = roleName });
        db.SaveChanges();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="roleName">a role</param>
    /// <param name="throwOnPopulatedRole">get upset of users are in a role</param>
    /// <returns></returns>
    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        try
        {
            var rl = db.Roles.FirstOrDefault(m => m.ID == roleName);
            db.Roles.Remove(rl);
            db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// required implemention
    /// </summary>
    /// <param name="roleName">a role</param>
    /// <param name="usernameToMatch">a username to look for in the role</param>
    /// <returns></returns>
    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        return db.UsersRoles.Where(m => m.Role == roleName && m.User.Contains(usernameToMatch)).Select(m => m.User).ToArray<string>();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <returns></returns>
    public override string[] GetAllRoles()
    {
        return db.Roles.Select(m => m.ID).ToArray<string>();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="username">a username</param>
    /// <returns>a list of roles</returns>
    public override string[] GetRolesForUser(string username)
    {
        return db.UsersRoles.Where(m => m.User == username).Select(m => m.Role).ToArray<string>();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="roleName">a role</param>
    /// <returns>a list of users</returns>
    public override string[] GetUsersInRole(string roleName)
    {
        return db.UsersRoles.Where(m => m.Role == roleName).Select(m => m.User).ToArray<string>();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="username">a username</param>
    /// <param name="roleName">a role</param>
    /// <returns>true or false</returns>
    public override bool IsUserInRole(string username, string roleName)
    {
        return db.UsersRoles.Count(m => m.User == username && m.Role == roleName) > 0;
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="usernames">a list of usernames</param>
    /// <param name="roleNames">a list of roles</param>
    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        foreach (var user in usernames)
        {
            foreach (var role in roleNames)
            {
                var qund = db.UsersRoles.FirstOrDefault(m => m.Role == role && m.User == user);
                db.UsersRoles.Remove(qund);
            }
        }
        db.SaveChanges();
    }

    /// <summary>
    /// required implementation
    /// </summary>
    /// <param name="roleName">a role</param>
    /// <returns>true or false</returns>
    public override bool RoleExists(string roleName)
    {
        return db.Roles.Count(m => m.ID == roleName) > 0;
    }
}