using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Models;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class AnnualMeetingController : BaseController
	{
		private readonly Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /AnnualMeeting/

		public ActionResult Index()
		{
			return View();
		}

		//
		// GET: /AnnualMeeting/AdminAnnualMeetings

		[Authorize]
		public ActionResult AdminAnnualMeetings()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var annualMeetings = _db.AnnualMeetings.OrderByDescending(b => b.DateCreated).ToList();

			return View(annualMeetings);
		}

		//
		// GET: /AnnualMeeting/Create

		public ActionResult Create()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			return View();
		}

		//
		// POST: /AnnualMeeting/Create

		[HttpPost]
		public ActionResult Create(AnnualMeeting annualMeeting, HttpPostedFileBase file)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if ((file != null) && (file.ContentLength > 0))
			{
				var errorAr = AnnualMeetingHelper.ValidateAnnualMeeting(annualMeeting);
				if (errorAr == ErrorCodes.EditErrorCodes.Success)
				{
					var error = AnnualMeetingHelper.ValidateFileName(file.FileName);
					if (error == ErrorCodes.FileNameErrorCodes.Success)
					{
						// Set time to now
						annualMeeting.DateCreated = DateTime.Now;
						annualMeeting.FileSize = file.ContentLength;
						annualMeeting.FileType = FileHelper.GetFileFormatFromContentType(file.ContentType).ToString();

						// Create upload path
						var fileName = Path.GetFileName(file.FileName);
						if (fileName == null)
						{ return View(annualMeeting); }

						var path = Path.Combine(Server.MapPath(Constants.FileUploadPath + Constants.AnnualMeetingUploadPath),
							fileName);

						annualMeeting.FilePath = Constants.FileLinkPath + Constants.AnnualMeetingUploadPath + file.FileName;

						try
						{
							// Save the file
							file.SaveAs(path);

							_db.AnnualMeetings.Add(annualMeeting);
							_db.SaveChanges();

							// Redirect to AdminAnnouncements
							return RedirectToAction("AdminAnnualMeetings", "AnnualMeeting");
						}
						catch (DirectoryNotFoundException)
						{
							ModelState.AddModelError("", Resources.AnnualMeeting.Create.ERROR_INTERNAL_PATH_NOT_FOUND);
						}
					}
					else
					{
						// Filename error
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(error));
					}
				}
				else
				{
					// AnnualMeeting object error
					ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(errorAr));
				}
			}
			else
			{
				ModelState.AddModelError("", Resources.AnnualMeeting.Create.ERROR_INTERNAL_NOT_SAVE_MODEL);
			}


			return View(annualMeeting);
		}

		//
		// GET: /AnnualMeeting/Edit/5

		public ActionResult Edit(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			/**
			 * Fill Model
			 */
			var annualMeeting = _db.AnnualMeetings.FirstOrDefault(a => a.Id == id);

			return View(annualMeeting);
		}

		//
		// POST: /AnnualMeeting/EditUnionInfo/5

		[HttpPost]
		public ActionResult Edit(AnnualMeeting annualMeeting)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbannualMeeting = _db.AnnualMeetings.FirstOrDefault(a => a.Id == annualMeeting.Id);
				if (dbannualMeeting != null)
				{
					// Validate the user
					var status = AnnualMeetingHelper.ValidateChangedAnnualMeeting(dbannualMeeting);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbannualMeeting))
						{
							_db.Entry(dbannualMeeting).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditAnnualMeetingSuccess"] = Resources.AnnualMeeting.Edit.SUCCESS_ANNUAL_MEETING_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.AnnualMeeting.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.AnnualMeeting.Edit.ERROR_INTERNAL_NOT_FIND_ANNUAL_MEETING);
				}
			}
			return View(annualMeeting);
		}

		//
		// GET: /AnnualMeeting/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var annualMeeting = _db.AnnualMeetings.Find(id);
			return View(annualMeeting);
		}

		//
		// POST: /AnnualMeeting/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Find the annual report
			var annualMeeting = _db.AnnualMeetings.Find(id);

			try
			{
				// Gain access to FTP and delete the file
				var requestFileDelete = (FtpWebRequest)WebRequest.Create(Constants.FileFtpLink + annualMeeting.FilePath);
				requestFileDelete.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUser"],
					ConfigurationManager.AppSettings["FTPPassword"]);
				requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
				using ((FtpWebResponse) requestFileDelete.GetResponse())
				{
					// Delete the AnnualMeeting
					_db.AnnualMeetings.Remove(annualMeeting);
					_db.SaveChanges();

					return RedirectToAction("AdminAnnualMeetings", "AnnualMeeting");
				}
			}
			catch (Exception)
			{
				ModelState.AddModelError("", Resources.ErrorCodes.ERROR_LOGIN_FTP);
			}

			return View(annualMeeting);
		}
	}
}