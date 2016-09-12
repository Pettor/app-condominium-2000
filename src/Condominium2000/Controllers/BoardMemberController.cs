using System;
using System.Linq;
using System.Web.Mvc;
using Condominium2000.Controllers.Interface;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Models;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using EntityState = System.Data.Entity.EntityState;

namespace Condominium2000.Controllers
{
	public class BoardMemberController : BaseController
	{
		private readonly Condominium2000Context db = new Condominium2000Context();

		//
		// GET: /BoardMember/

		public ActionResult Index()
		{
			return View();
		}


		//
		// GET: /News/AdminBoardMembers

		[Authorize]
		public ActionResult AdminBoardMembers()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
             * Fill Model
             */
			var boardMembers = db.BoardMembers.OrderBy(b => b.ListPriority).ToList();

			return View(boardMembers);
		}

		//
		// GET: /BoardMember/Create

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
		// POST: /BoardMember/Create

		[HttpPost]
		public ActionResult Create(BoardMember boardMember)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if (ModelState.IsValid)
			{
				// Set time to now
				boardMember.DateCreated = DateTime.Now;

				db.BoardMembers.Add(boardMember);
				db.SaveChanges();

				// Redirect to AdminAnnouncements
				return RedirectToAction("AdminBoardMembers", "BoardMember");
			}

			return View(boardMember);
		}

		//
		// GET: /BoardMember/Edit/5

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
			var boardMember = db.BoardMembers.FirstOrDefault(a => a.Id == id);

			return View(boardMember);
		}

		//
		// POST: /BoardMember/Edit/5

		[HttpPost]
		public ActionResult Edit(BoardMember boardMember)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbBoardMember = db.BoardMembers.FirstOrDefault(a => a.Id == boardMember.Id);
				if (dbBoardMember != null)
				{
					// Validate the user
					var status = BoardMemberHelper.ValidateChangedBoardMember(dbBoardMember);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbBoardMember))
						{
							db.Entry(dbBoardMember).State = EntityState.Modified;
							db.SaveChanges();
							TempData["EditBoardMemberSuccess"] = Resources.BoardMember.Edit.SUCCESS_BOARD_MEMBER_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.BoardMember.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.BoardMember.Edit.ERROR_INTERNAL_NOT_FIND_BOARD_MEMBER);
				}
			}
			return View(boardMember);
		}

		//
		// GET: /BoardMember/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var boardMember = db.BoardMembers.Find(id);
			return View(boardMember);
		}

		//
		// POST: /BoardMember/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var boardMember = db.BoardMembers.Find(id);
			db.BoardMembers.Remove(boardMember);
			db.SaveChanges();

			return RedirectToAction("AdminBoardMembers", "BoardMember");
		}
	}
}