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
	public class QuestionController : BaseController
	{
		private readonly Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /Question/

		public ActionResult Index()
		{
			return View();
		}


		//
		// GET: /News/AdminQuestions

		[Authorize]
		public ActionResult AdminQuestions()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var questions = _db.Questions.OrderBy(b => b.ListPriority).ToList();

			return View(questions);
		}

		//
		// GET: /Question/Create

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
		// POST: /Question/Create

		[HttpPost]
		public ActionResult Create(Question question)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				// Set time to now
				question.DateCreated = DateTime.Now;

				_db.Questions.Add(question);
				_db.SaveChanges();

				// Redirect to AdminAnnouncements
				return RedirectToAction("AdminQuestions", "Question");
			}

			return View(question);
		}

		//
		// GET: /Society/Edit/5

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
			var question = _db.Questions.FirstOrDefault(a => a.Id == id);

			return View(question);
		}

		//
		// POST: /Society/EditUnionInfo/5

		[HttpPost]
		public ActionResult Edit(Question question)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbQuestion = _db.Questions.FirstOrDefault(a => a.Id == question.Id);
				if (dbQuestion != null)
				{
					// Validate the user
					var status = QuestionHelper.ValidateChangedQuestion(dbQuestion);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbQuestion))
						{
							// Save HTML content
							dbQuestion.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(question.ContentEn);
							dbQuestion.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(question.ContentSv);

							_db.Entry(dbQuestion).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditQuestionSuccess"] = Resources.Question.Edit.SUCCESS_QUESTION_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.Question.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.Question.Edit.ERROR_INTERNAL_NOT_FIND_QUESTION);
				}
			}
			return View(question);
		}

		//
		// GET: /Question/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var question = _db.Questions.Find(id);
			return View(question);
		}

		//
		// POST: /Question/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var question = _db.Questions.Find(id);
			_db.Questions.Remove(question);
			_db.SaveChanges();

			return RedirectToAction("AdminQuestions", "Question");
		}

		//
		// GET: /Announcement/Preview/5

		public ActionResult Preview(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var quest = _db.Questions.Find(id);

			// Convert Input to valid HTML
			quest.HtmlContentSv = ConvertTagToHtml.ConvertInputToHtml(quest.ContentSv);
			quest.HtmlContentEn = ConvertTagToHtml.ConvertInputToHtml(quest.ContentEn);
			return View(quest);
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
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}