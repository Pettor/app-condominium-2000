using System.Linq;
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
using Condominium2000.Resources.Resident;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class ResidentController : BaseController
	{
		private Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /Resident/

		public ActionResult Index()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var residentTemplate = new ResidentViewModel(ref _db);

			return View(residentTemplate);
		}

		//
		// GET: /Resident/Questions

		public ActionResult Questions()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var residentTemplate = new ResidentViewModel(ref _db);

			return View(residentTemplate);
		}

		//
		// GET: /Resident/ShowQuestion

		public ActionResult ShowQuestion(int id)
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Get the selected news
			var question = _db.Questions.FirstOrDefault(n => n.Id == id);

			return View(question);
		}

		//
		// GET: /Resident/Gallery

		public ActionResult Gallery()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var residentTemplate = new ResidentViewModel(ref _db);

			return View(residentTemplate);
		}

		//
		// GET: /Resident/Links

		public ActionResult Links()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var residentTemplate = new ResidentViewModel(ref _db);

			return View(residentTemplate);
		}

		//
		// GET: /Resident/BookRoom

		public ActionResult BookRoom()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.Resident;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var residentTemplate = new ResidentViewModel(ref _db);

			return View(residentTemplate);
		}


		//
		// GET: /Resident/EditResidentInfo/5

		public ActionResult EditResidentInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			/**
			 * Fill Model
			 */
			var si = _db.ResidentInfos.FirstOrDefault(a => a.Id == id);

			return View(si);
		}

		//
		// POST: /Resident/EditResidentInfo/5

		[HttpPost]
		public ActionResult EditResidentInfo(ResidentInfo ri)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			if (ModelState.IsValid)
			{
				var dbRi = _db.ResidentInfos.FirstOrDefault(a => a.Id == ri.Id);
				if (dbRi != null)
				{
					// Validate the user
					var status = ResidentHelper.ValidateChangedResidentInfo(dbRi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbRi))
						{
							// Save HTML content
							dbRi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(ri.ContentEn);
							dbRi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(ri.ContentSv);

							_db.Entry(dbRi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
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
					ModelState.AddModelError("", Resources.Society.EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(ri);
		}

		//
		// GET: /Resident/PreviewLinksInfo/5

		public ActionResult PreviewResidentInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.ResidentInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// GET: /Resident/EditQuestionsInfo/5

		public ActionResult EditQuestionsInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			/**
			 * Fill Model
			 */
			var si = _db.QuestionsInfos.FirstOrDefault(a => a.Id == id);

			return View(si);
		}

		//
		// POST: /Resident/EditQuestionsInfo/5

		[HttpPost]
		public ActionResult EditQuestionsInfo(QuestionsInfo qi)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			if (ModelState.IsValid)
			{
				var dbQi = _db.QuestionsInfos.FirstOrDefault(a => a.Id == qi.Id);
				if (dbQi != null)
				{
					// Validate the user
					var status = ResidentHelper.ValidateChangedQuestionsInfo(dbQi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbQi))
						{
							// Save HTML content
							dbQi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(qi.ContentEn);
							dbQi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(qi.ContentSv);

							_db.Entry(dbQi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
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
					ModelState.AddModelError("", Resources.Society.EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(qi);
		}

		//
		// GET: /Resident/PreviewQuestionsInfo/5

		public ActionResult PreviewQuestionsInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.QuestionsInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}

		//
		// GET: /Resident/EditLinksInfo/5

		public ActionResult EditLinksInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			/**
			 * Fill Model
			 */
			var si = _db.LinksInfos.FirstOrDefault(a => a.Id == id);

			return View(si);
		}

		//
		// POST: /Resident/EditLinksInfo/5

		[HttpPost]
		public ActionResult EditLinksInfo(LinksInfo li)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			if (ModelState.IsValid)
			{
				var dbLi = _db.LinksInfos.FirstOrDefault(a => a.Id == li.Id);
				if (dbLi != null)
				{
					// Validate the user
					var status = ResidentHelper.ValidateChangedLinksInfo(dbLi);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbLi))
						{
							// Save HTML content
							dbLi.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(li.ContentEn);
							dbLi.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(li.ContentSv);

							_db.Entry(dbLi).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
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
					ModelState.AddModelError("", Resources.Society.EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(li);
		}

		//
		// GET: /Resident/PreviewLinksInfo/5

		public ActionResult PreviewLinksInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.LinksInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
		}


		//
		// GET: /Resident/EditBookRoomInfo/5

		public ActionResult EditBookRoomInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			/**
			 * Fill Model
			 */
			var si = _db.BookRoomInfos.FirstOrDefault(a => a.Id == id);

			return View(si);
		}

		//
		// POST: /Resident/EditBookRoomInfo/5

		[HttpPost]
		public ActionResult EditBookRoomInfo(BookRoomInfo bri)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Resident");
			}

			if (ModelState.IsValid)
			{
				var dbBri = _db.BookRoomInfos.FirstOrDefault(a => a.Id == bri.Id);
				if (dbBri != null)
				{
					// Validate the user
					var status = ResidentHelper.ValidateChangedBookRoomInfo(dbBri);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbBri))
						{
							// Save HTML content
							dbBri.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(bri.ContentEn);
							dbBri.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(bri.ContentSv);

							_db.Entry(dbBri).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditInfoSuccess"] = EditInfo.SUCCESS_INFO_SAVED;
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
					ModelState.AddModelError("", Resources.Society.EditInfo.ERROR_INTERNAL_NOT_FIND_INFO);
				}
			}
			return View(bri);
		}

		//
		// GET: /Resident/PreviewBookRoom/5

		public ActionResult PreviewBookRoomInfo(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var info = _db.BookRoomInfos.Find(id);

			// Convert Input to valid HTML
			info.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(info.ContentSv);
			info.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(info.ContentEn);
			return View(info);
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