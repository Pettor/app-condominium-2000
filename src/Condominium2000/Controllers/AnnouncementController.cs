using System;
using System.Linq;
using System.Web.Mvc;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Models;
using Condominium2000.Helpers.Tags;
using Condominium2000.Helpers.Translator;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class AnnouncementController : BaseController
	{
		private Condominium2000Context db = new Condominium2000Context();

		//
		// GET: /Announcement/

		public ViewResult Index()
		{
			return View(db.Announcements.ToList());
		}

		//
		// GET: /Announcement/Details/5

		public ViewResult Details(int id)
		{
			var announcement = db.Announcements.Find(id);
			return View(announcement);
		}

		//
		// GET: /News/AdminAnnouncements

		[Authorize]
		public ActionResult AdminAnnouncements()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var viewModel = new EditSelectedAnnouncementModel(ref db);

			return View(viewModel);
		}

		//
		// POST: /News/AdminAnnouncements

		[Authorize]
		[HttpPost]
		public ActionResult AdminAnnouncements(EditSelectedAnnouncementModel announcementModel, FormCollection forms)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Get default NewsTemplate
			var template = TemplateHelper.GetNewsTemplate(ref db);

			// Check if selected announcement has been changed
			if (template.SelectedAnnouncement.Id != announcementModel.SelectedAnnouncementId)
			{
				// Get announcement and check if valid
				var announcement = db.Announcements.FirstOrDefault(a => a.Id == announcementModel.SelectedAnnouncementId);
				if (announcement != null)
				{
					// Save changes
					template.SelectedAnnouncement = announcement;
					db.SaveChanges();
					TempData["EditAnnouncementSuccess"] = Resources.Announcement.AdminAnnouncements.SUCCESS_SELECTED_ANNOUNCEMENT_SAVED;
				}
				else
				{
					ModelState.AddModelError("", Resources.Announcement.AdminAnnouncements.ERROR_INTERNAL_NOT_FOUND);
				}
			}

			/**
			 * Fill Model
			 */
			var viewModel = new EditSelectedAnnouncementModel(ref db);

			return View(viewModel);
		}

		//
		// GET: /Announcement/Create

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
		// POST: /Announcement/Create

		[HttpPost]
		public ActionResult Create(Announcement announcement)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				// Set time to now
				announcement.DateCreated = DateTime.Now;

				// Save HTML content
				announcement.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentEn);
				announcement.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentSv);

				db.Announcements.Add(announcement);
				db.SaveChanges();

				// Redirect to AdminAnnouncements
				return RedirectToAction("AdminAnnouncements");
			}

			return View(announcement);
		}

		//
		// GET: /Announcement/Edit/5

		public ActionResult Edit(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var ann = db.Announcements.FirstOrDefault(a => a.Id == id);

			return View(ann);
		}

		//
		// POST: /Announcement/Edit/5

		[HttpPost]
		public ActionResult Edit(Announcement announcement)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbAnnouncement = db.Announcements.FirstOrDefault(a => a.Id == announcement.Id);
				if (dbAnnouncement != null)
				{
					// Validate the user
					var status = AnnouncementHelper.ValidateChangedAnnouncement(dbAnnouncement);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbAnnouncement))
						{
							// Save HTML content
							dbAnnouncement.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentEn);
							dbAnnouncement.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentSv);

							db.Entry(dbAnnouncement).State = EntityState.Modified;
							db.SaveChanges();
							TempData["EditAnnouncementSuccess"] = Resources.Announcement.Edit.SUCCESS_ANNOUNCEMENT_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.Announcement.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.Announcement.Edit.ERROR_INTERNAL_NOT_FIND_ANNOUNCEMENT);
				}
			}
			return View(announcement);
		}

		//
		// GET: /Announcement/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var announcement = db.Announcements.Find(id);
			return View(announcement);
		}

		//
		// POST: /Announcement/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var announcement = db.Announcements.Find(id);
			if (db.Announcements.Count() > 1)
			{
				var nt = TemplateHelper.GetNewsTemplate(ref db);
				if (nt.SelectedAnnouncement.Id != announcement.Id)
				{
					db.Announcements.Remove(announcement);
					db.SaveChanges();
					return RedirectToAction("AdminAnnouncements");
				}
				ModelState.AddModelError("", Resources.Announcement.Delete.ERROR_DELETE_SELECTED);
			}
			else
			{
				ModelState.AddModelError("", Resources.Announcement.Delete.ERROR_DELETE_COUNT);
			}
			return View(announcement);
		}

		//
		// GET: /Announcement/Preview/5

		public ActionResult Preview(Announcement announcement)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			announcement.DateCreated = DateTime.Now;

			// Convert Input to valid HTML
			announcement.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentSv);
			announcement.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(announcement.ContentEn);
			return View(announcement);
		}


		//
		// POST: /Announcement/TranslateText
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
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}