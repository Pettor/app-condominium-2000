using System;
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
using PagedList;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class NewsController : BaseController
	{
		private Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /News/

		public ActionResult Index()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.News;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var viewModel = new NewsViewModel(ref _db, NewsViewModel.DisplayNews.Limited);

			/**
			 * Return View
			 */
			return View(viewModel);
		}

		//
		// GET: /News/Show/5

		public ActionResult Show(int id)
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.News;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var viewModel = new NewsViewModel(ref _db, NewsViewModel.DisplayNews.All);
			// Get the selected news
			viewModel.SelectedNews = viewModel.News.FirstOrDefault(n => n.Id == id);

			/**
			 * Return View
			 */
			return View(viewModel);
		}

		//
		// GET: /ShowAll/

		public ActionResult ShowAll(int? page)
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.News;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var viewModel = new NewsViewModel(ref _db, NewsViewModel.DisplayNews.All);

			// Set news Pagination variable
			var pageSize = Constants.NewsShowAllPaginateSize;
			var pageNumber = page ?? 1;
			// Fill the paginated list
			viewModel.PaginationList = viewModel.News.OrderByDescending(n => n.DateCreated).ToPagedList(pageNumber, pageSize);

			/**
			 * Return View
			 */
			return View(viewModel);
		}

		//
		// GET: /News/Create

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
		// POST: /News/Create

		[HttpPost]
		public ActionResult Create(News news)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				// Set time to now
				news.DateCreated = DateTime.Now;

				// News written by logged in user
				news.WrittenBy = UserHelper.GetUser(ref _db, User.UserName);

				// Save HTML content
				news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
				news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);

				_db.News.Add(news);
				_db.SaveChanges();

				// Redirect to AdminAnnouncements
				return RedirectToAction("Index", "News");
			}

			return View(news);
		}

		//
		// GET: /News/Edit/5

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
			var news = _db.News.FirstOrDefault(a => a.Id == id);

			return View(news);
		}

		//
		// POST: /News/Edit/5

		[HttpPost]
		public ActionResult Edit(News news)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				var dbNews = _db.News.FirstOrDefault(a => a.Id == news.Id);
				if (dbNews != null)
				{
					// Validate the user
					var status = NewsHelper.ValidateChangedNews(dbNews);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbNews))
						{
							// Save HTML content
							dbNews.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);
							dbNews.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);

							_db.Entry(dbNews).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditNewsSuccess"] = Resources.News.Edit.SUCCESS_NEWS_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.News.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.News.Edit.ERROR_INTERNAL_NOT_FIND_NEWS);
				}
			}
			return View(news);
		}

		//
		// GET: /News/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.News.Find(id);
			return View(news);
		}

		//
		// POST: /News/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var news = _db.News.Find(id);
			if (_db.News.Count() > 1)
			{
				_db.News.Remove(news);
				_db.SaveChanges();
				return RedirectToAction("Index", "News");
			}
			ModelState.AddModelError("", Resources.Announcement.Delete.ERROR_DELETE_COUNT);
			return View(news);
		}

		//
		// GET: /News/Preview/5
		[HttpPost]
		public ActionResult Preview(News news)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			news.DateCreated = DateTime.Now;

			news.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(news.ContentSv);
			news.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(news.ContentEn);

			return View(news);
		}

		//
		// POST: /News/TranslateText
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