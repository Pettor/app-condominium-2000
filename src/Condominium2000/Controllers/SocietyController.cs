using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Models;
using Condominium2000.Helpers.Session;
using Condominium2000.Helpers.Tags;
using Condominium2000.Helpers.Translator;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using Condominium2000.Models.ViewModels;
using Condominium2000.Resources.Society;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class SocietyController : BaseController
	{
		private Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /Society/

		public ActionResult Index()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Society;

			/**
			 * Fill Model
			 */
			var societyTemplate = new SocietyViewModel(ref _db);

			return View(societyTemplate);
		}

		//
		// GET: /Society/Union

		public ActionResult Union()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Society;

			/**
			 * Fill Model
			 */
			var societyTemplate = new SocietyViewModel(ref _db);

			return View(societyTemplate);
		}

		//
		// GET: /Society/AnnualReports

		public ActionResult AnnualReports()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Society;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var societyTemplate = new SocietyViewModel(ref _db);

			return View(societyTemplate);
		}

		[HttpPost]
		public ActionResult AnnualReports(FormCollection formcollection, HttpPostedFileBase file)
		{
			if (file.ContentLength > 0)
			{
				var fileName = Path.GetFileName(file.FileName);
				if (fileName == null)
				{ return RedirectToAction("AnnualReports"); }

				var path = Path.Combine(Server.MapPath("~/Uploads/AnnualReports/"), fileName);
				file.SaveAs(path);
			}

			return RedirectToAction("AnnualReports");
		}

		//
		// GET: /Society/AnnualMeetings

		public ActionResult AnnualMeetings()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Society;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var societyTemplate = new SocietyViewModel(ref _db);

			return View(societyTemplate);
		}

		//
		// GET: /Society/EditUnionInfo/5

		public ActionResult EditUnionInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			/**
			 * Fill Model
			 */
			var ui = _db.UnionInfos.FirstOrDefault(a => a.Id == id);

			return View(ui);
		}

		//
		// POST: /Society/EditUnionInfo/5

		[HttpPost]
		public ActionResult EditUnionInfo(UnionInfo ui)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbUi = _db.UnionInfos.FirstOrDefault(a => a.Id == ui.Id);
				if (dbUi != null)
				{
					// Validate the user
					var status = SocietyHelper.ValidateChangedUnionInfo(dbUi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbUi))
						{
							// Save HTML content
							dbUi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ui.ContentEn);
							dbUi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ui.ContentSv);

							_db.Entry(dbUi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditUnionInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(ui);
		}

		//
		// GET: /Society/PreviewUnionInfo/5

		public ActionResult Preview(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.UnionInfos.Find(id);

			// Convert Input to valid HTML
			news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);
			news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
			return View(news);
		}

		//
		// GET: /Society/EditSocietyInfo/5

		public ActionResult EditSocietyInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			/**
			 * Fill Model
			 */
			var si = _db.SocietyInfos.FirstOrDefault(a => a.Id == id);

			return View(si);
		}

		//
		// POST: /Society/EditSocietyInfo/5

		[HttpPost]
		public ActionResult EditSocietyInfo(SocietyInfo si)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbSi = _db.SocietyInfos.FirstOrDefault(a => a.Id == si.Id);
				if (dbSi != null)
				{
					// Validate the user
					var status = SocietyHelper.ValidateChangedSocietyInfo(dbSi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbSi))
						{
							// Save HTML content
							dbSi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(si.ContentEn);
							dbSi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(si.ContentSv);

							_db.Entry(dbSi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditSocietyInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(si);
		}

		//
		// GET: /Society/PreviewSocietyInfo/5

		public ActionResult PreviewSocietyInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.SocietyInfos.Find(id);

			// Convert Input to valid HTML
			news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);
			news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
			return View(news);
		}

		//
		// GET: /Society/EditAnnualReportsInfo/5

		public ActionResult EditAnnualReportInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			/**
			 * Fill Model
			 */
			var ar = _db.AnnualReportInfos.FirstOrDefault(a => a.Id == id);

			return View(ar);
		}

		//
		// POST: /Society/EditAnnualReportsInfo/5

		[HttpPost]
		public ActionResult EditAnnualReportInfo(AnnualReportInfo ar)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbAr = _db.AnnualReportInfos.FirstOrDefault(a => a.Id == ar.Id);
				if (dbAr != null)
				{
					// Validate the user
					var status = SocietyHelper.ValidateChangedAnnualReportInfo(dbAr);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbAr))
						{
							// Save HTML content
							dbAr.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ar.ContentEn);
							dbAr.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ar.ContentSv);

							_db.Entry(dbAr).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditAnnualReportInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(ar);
		}

		//
		// GET: /Society/PreviewAnnualReportInfo/5

		public ActionResult PreviewAnnualReportInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.AnnualReportInfos.Find(id);

			// Convert Input to valid HTML
			news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);
			news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
			return View(news);
		}

		//
		// GET: /Society/EditAnnualMeetingInfo/5

		public ActionResult EditAnnualMeetingInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			/**
			 * Fill Model
			 */
			var am = _db.AnnualMeetingInfos.FirstOrDefault(a => a.Id == id);

			return View(am);
		}

		//
		// POST: /Society/EditAnnualMeetingInfo/5

		[HttpPost]
		public ActionResult EditAnnualMeetingInfo(AnnualMeetingInfo am)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbAm = _db.AnnualMeetingInfos.FirstOrDefault(a => a.Id == am.Id);
				if (dbAm != null)
				{
					// Validate the user
					var status = SocietyHelper.ValidateChangedAnnualMeetingInfo(dbAm);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbAm))
						{
							// Save HTML content
							dbAm.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(am.ContentEn);
							dbAm.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(am.ContentSv);

							_db.Entry(dbAm).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditAnnualMeetingInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
						}
						else
						{
							ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(am);
		}

		//
		// GET: /Society/PreviewAnnualMeetingInfo/5

		public ActionResult PreviewAnnualMeetingInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.AnnualMeetingInfos.Find(id);

			// Convert Input to valid HTML
			news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);
			news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
			return View(news);
		}

		//
		// POST: /Society/TranslateText
		// AJAX Operation

		[HttpPost]
		public ActionResult TranslateText(GoogleTranslator.TranslateObject obj)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (obj.Lang == null)
			{
				return Json(obj);
			}

			return Json(TranslationHelper.TranslateText(obj));
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}