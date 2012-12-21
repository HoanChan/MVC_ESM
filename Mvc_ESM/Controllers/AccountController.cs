using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc_ESM.Models;

namespace Mvc_ESM.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Roles.Contains("Admin") && model.Roles.Contains("GiaoVien") && model.RolePass != "Admin GiaoVien")
                {
                    ModelState.AddModelError("", "Không thể được cấp quyền Admin và GiaoVien nếu không nhập đúng mã xác nhận!");
                    return View(model);
                }

                if (model.Roles.Contains("Admin") && model.RolePass != "Admin")
                {
                    ModelState.AddModelError("", "Không thể được cấp quyền Admin nếu không nhập đúng mã xác nhận!");
                    return View(model);
                }

                if (model.Roles.Contains("GiaoVien") && model.RolePass != "GiaoVien")
                {
                    ModelState.AddModelError("", "Không thể được cấp quyền GiaoVien nếu không nhập đúng mã xác nhận!");
                    return View(model);
                }

                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "Question", "Answer", true, null, out createStatus);
                
                if (createStatus == MembershipCreateStatus.Success)
                {
                    foreach(var role in model.Roles)
                    {
                        Roles.AddUserToRole(model.UserName, role);
                    }
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại sai hoặc mật khẩu mới không đúng.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Người dùng đã tồn tại. Đề nghị nhập tên đăng nhập khác.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email này đã có người sử dụng. Đề nghị nhập địa chỉ Email khác.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Mật khẩu không không hợp lệ. Hãy nhập lại cho đúng.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Địa chỉ Email không hợp lệ. Hãy kiểm tra và nhập lại.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Tên đăng nhập không hợp lệ. Hãy kiểm tra và thử lại.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "Một lỗi không khác định được đã xảy ra. Kiểm tra lại các thông số và thử lại. Nếu vẫn còn lỗi, liên hệ với quản trị viên.";
            }
        }
        #endregion
    }
}
