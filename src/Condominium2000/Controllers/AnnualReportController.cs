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
	public class AnnualReportController : BaseController
	{
		private readonly Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /AnnualReport/

		public ActionResult Index()
		{
			return View();
		}


		//
		// GET: /AnnualReport/AdminAnnualReports

		[Authorize]
		public ActionResult AdminAnnualReports()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var annualReports = _db.AnnualReports.OrderByDescending(b => b.DateCreated).ToList();

			return View(annualReports);
		}

		//
		// GET: /AnnualReport/Create

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
		// POST: /AnnualReport/Create

		[HttpPost]
		public ActionResult Create(AnnualReport annualReport, HttpPostedFileBase file)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if ((file != null) && (file.ContentLength > 0))
			{
				var errorAr = AnnualReportHelper.ValidateAnnualReport(annualReport);
				if (errorAr == ErrorCodes.EditErrorCodes.Success)
				{
					var error = AnnualReportHelper.ValidateFileName(file.FileName);
					if (error == ErrorCodes.FileNameErrorCodes.Success)
					{
						// Set time to now
						annualReport.DateCreated = DateTime.Now;
						annualReport.FileSize = file.ContentLength;
						annualReport.FileType = FileHelper.GetFileFormatFromContentType(file.ContentType).ToString();

						// Create upload path
						var fileName = Path.GetFileName(file.FileName);
						if (fileName == null)
						{ return View(annualReport); }

						var path = Path.Combine(Server.MapPath(Constants.FileUploadPath + Constants.AnnualReportUploadPath), fileName);
						annualReport.FilePath = Constants.FileLinkPath + Constants.AnnualReportUploadPath + file.FileName;

						try
						{
							// Save the file
							file.SaveAs(path);

							_db.AnnualReports.Add(annualReport);
							_db.SaveChanges();

							// Redirect to AdminAnnouncements
							return RedirectToAction("AdminAnnualReports", "AnnualReport");
						}
						catch (DirectoryNotFoundException)
						{
							ModelState.AddModelError("", Resources.AnnualReport.Create.ERROR_INTERNAL_PATH_NOT_FOUND);
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
					// AnnualReport object error
					ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(errorAr));
				}
			}
			else
			{
				ModelState.AddModelError("", Resources.AnnualReport.Create.ERROR_INTERNAL_NOT_SAVE_MODEL);
			}


			return View(annualReport);
		}

		//
		// GET: /AnnualReport/Edit/5

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
			var annualReport = _db.AnnualReports.FirstOrDefault(a => a.Id == id);

			return View(annualReport);
		}

		//
		// POST: /AnnualReport/EditUnionInfo/5

		[HttpPost]
		public ActionResult Edit(AnnualReport annualReport)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbannualReport = _db.AnnualReports.FirstOrDefault(a => a.Id == annualReport.Id);
				if (dbannualReport != null)
				{
					// Validate the user
					var status = AnnualReportHelper.ValidateChangedAnnualReport(dbannualReport);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbannualReport))
						{
							_db.Entry(dbannualReport).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditAnnualReportSuccess"] = Resources.AnnualReport.Edit.SUCCESS_ANNUAL_REPORT_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.AnnualReport.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.AnnualReport.Edit.ERROR_INTERNAL_NOT_FIND_ANNUAL_REPORT);
				}
			}
			return View(annualReport);
		}

		//
		// GET: /AnnualReport/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var annualReport = _db.AnnualReports.Find(id);
			return View(annualReport);
		}

		//
		// POST: /AnnualReport/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Find the annual report
			var annualReport = _db.AnnualReports.Find(id);

			try
			{
				// Gain access to FTP and delete the file
				var requestFileDelete = (FtpWebRequest)WebRequest.Create(Constants.FileFtpLink + annualReport.FilePath);
				requestFileDelete.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUser"],
					ConfigurationManager.AppSettings["FTPPassword"]);
				requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
				using ((FtpWebResponse) requestFileDelete.GetResponse())
				{
					// Delete the AnnualReport
					_db.AnnualReports.Remove(annualReport);
					_db.SaveChanges();

					return RedirectToAction("AdminAnnualReports", "AnnualReport");
				}
			}
			catch (Exception)
			{
				ModelState.AddModelError("", Resources.ErrorCodes.ERROR_LOGIN_FTP);
			}

			return View(annualReport);
		}
	}
}