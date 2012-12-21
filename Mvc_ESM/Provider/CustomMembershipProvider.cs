using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Web.Security;
using Model;
using System.Text.RegularExpressions;

public class CustomMembershipProvider : System.Web.Security.MembershipProvider
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

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        var args = new ValidatePasswordEventArgs(username, password, true);
        OnValidatingPassword(args);

        if (args.Cancel)
        {
            status = MembershipCreateStatus.InvalidPassword;
            return null;
        }

        if (!Regex.IsMatch(email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$"))
        {
            status = MembershipCreateStatus.InvalidEmail;
            return null;
        }

        if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
        {
            status = MembershipCreateStatus.DuplicateEmail;
            return null;
        }

        var user = GetUser(username, true);

        if (user == null)
        {
            string MD5Pass = GetMd5Hash(password);
            var userObj = new User { ID = username, Name = username, Pass = MD5Pass, Email = email, Question = passwordQuestion, Answer = passwordAnswer,LastLogin = DateTime.Now };

            db.Users.Add(userObj);
            db.SaveChanges();

            status = MembershipCreateStatus.Success;

            return GetUser(username, true);
        }
        status = MembershipCreateStatus.DuplicateUserName;

        return null;
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        throw new NotImplementedException();
    }

    public override bool EnablePasswordReset
    {
        get { throw new NotImplementedException(); }
    }

    public override bool EnablePasswordRetrieval
    {
        get { throw new NotImplementedException(); }
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override int GetNumberOfUsersOnline()
    {
        throw new NotImplementedException();
    }

    public override string GetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        var user = db.Users.FirstOrDefault(m=>m.ID == username);
        if (user != null)
        {
            var memUser = new MembershipUser(Membership.Provider.Name, username, user.ID + user.Pass, user.Email,
                                                        string.Empty, string.Empty,
                                                        true, false, DateTime.MinValue,
                                                        user.LastLogin.Value,
                                                        DateTime.MinValue,
                                                        DateTime.Now, DateTime.Now);
            return memUser;
        }
        return null;
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        throw new NotImplementedException();
    }

    public override string GetUserNameByEmail(string email)
    {
        var aUser = db.Users.FirstOrDefault(m => m.Email == email);
        return aUser == null ? string.Empty : aUser.Email;
    }

    public override int MaxInvalidPasswordAttempts
    {
        get { throw new NotImplementedException(); }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { throw new NotImplementedException(); }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 6; }
    }

    public override int PasswordAttemptWindow
    {
        get { throw new NotImplementedException(); }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { throw new NotImplementedException(); }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { throw new NotImplementedException(); }
    }

    public override bool RequiresQuestionAndAnswer
    {
        get { throw new NotImplementedException(); }
    }

    public override bool RequiresUniqueEmail
    {
        get { return true; }
    }

    public override string ResetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override bool UnlockUser(string userName)
    {
        throw new NotImplementedException();
    }

    public override void UpdateUser(MembershipUser user)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateUser(string username, string password)
    {
        string MD5Pass = GetMd5Hash(password);
        var requiredUser = db.Users.FirstOrDefault(m => m.ID == username && m.Pass == MD5Pass);
        return requiredUser != null;
    }

    public static string GetMd5Hash(string value)
    {
        var md5Hasher = MD5.Create();
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
        var sBuilder = new StringBuilder();
        for (var i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }
}