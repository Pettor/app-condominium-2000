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
	public class GalleryImageController : BaseController
	{
		private readonly Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /GalleryImage/

		public ActionResult Index()
		{
			return View();
		}


		//
		// GET: /GalleryImage/AdminGalleryImages

		[Authorize]
		public ActionResult AdminGalleryImages()
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			/**
			 * Fill Model
			 */
			var galleryImages = _db.GalleryImages.OrderBy(b => b.ListPriority).ToList();

			return View(galleryImages);
		}

		//
		// GET: /GalleryImage/Create

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
		// POST: /GalleryImage/Create

		[HttpPost]
		public ActionResult Create(GalleryImage galleryImage, HttpPostedFileBase file)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			if ((file != null) && (file.ContentLength > 0))
			{
				if (GalleryImageHelper.ValidateGalleryImageType(file.ContentType))
				{
					var error = GalleryImageHelper.ValidateFileName(file.FileName);
					if (error == ErrorCodes.FileNameErrorCodes.Success)
					{
						// Set time to now
						galleryImage.DateCreated = DateTime.Now;
						galleryImage.FileSize = file.ContentLength;
						galleryImage.FileType = FileHelper.GetFileFormatFromContentType(file.ContentType).ToString();

						// Create upload path
						var fileName = Path.GetFileName(file.FileName);
						if (fileName == null)
						{ return View(galleryImage); }

						var path = Path.Combine(Server.MapPath(Constants.FileUploadPath + Constants.GalleryImageUploadPath), fileName);
						galleryImage.FilePath = Constants.FileLinkPath + Constants.GalleryImageUploadPath + file.FileName;

						try
						{
							// Save the file
							file.SaveAs(path);

							_db.GalleryImages.Add(galleryImage);
							_db.SaveChanges();

							// Redirect to AdminAnnouncements
							return RedirectToAction("AdminGalleryImages", "GalleryImage");
						}
						catch (DirectoryNotFoundException)
						{
							ModelState.AddModelError("", Resources.GalleryImage.Create.ERROR_INTERNAL_PATH_NOT_FOUND);
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
					// GalleryImage type error
					ModelState.AddModelError("", Resources.GalleryImage.Create.ERROR_INTERNAL_NOT_IMAGE_TYPE);
				}
			}
			else
			{
				ModelState.AddModelError("", Resources.GalleryImage.Create.ERROR_INTERNAL_NOT_SAVE_MODEL);
			}


			return View(galleryImage);
		}

		//
		// GET: /GalleryImage/Edit/5

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
			var galleryImage = _db.GalleryImages.FirstOrDefault(a => a.Id == id);

			return View(galleryImage);
		}

		//
		// POST: /GalleryImage/EditUnionInfo/5

		[HttpPost]
		public ActionResult Edit(GalleryImage galleryImage)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "Society");
			}

			if (ModelState.IsValid)
			{
				var dbgalleryImage = _db.GalleryImages.FirstOrDefault(a => a.Id == galleryImage.Id);
				if (dbgalleryImage != null)
				{
					// Validate the user
					var status = GalleryImageHelper.ValidateChangedGalleryImage(dbgalleryImage);

					if (status == ErrorCodes.EditErrorCodes.Success)
					{
						if (TryUpdateModel(dbgalleryImage))
						{
							_db.Entry(dbgalleryImage).State = EntityState.Modified;
							_db.SaveChanges();
							TempData["EditGalleryImageSuccess"] = Resources.GalleryImage.Edit.SUCCESS_GALLERY_IMAGE_SAVED;
						}
						else
						{
							ModelState.AddModelError("", Resources.GalleryImage.Edit.ERROR_INTERNAL_NOT_SAVE_MODEL);
						}
					}
					else
					{
						ModelState.AddModelError("", ErrorCodes.ErrorCodeToString(status));
					}
				}
				else
				{
					ModelState.AddModelError("", Resources.GalleryImage.Edit.ERROR_INTERNAL_NOT_FIND_GALLERY_IMAGE);
				}
			}
			return View(galleryImage);
		}

		//
		// GET: /GalleryImage/Delete/5

		public ActionResult Delete(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			var galleryImage = _db.GalleryImages.Find(id);
			return View(galleryImage);
		}

		//
		// POST: /GalleryImage/Delete/5

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			// Must be Admin to gain access to this functionality
			if (!User.IsAdmin())
			{
				return RedirectToAction("Index", "News");
			}

			// Find the annual report
			var galleryImage = _db.GalleryImages.Find(id);

			try
			{
				// Gain access to FTP and delete the file
				var requestFileDelete = (FtpWebRequest)WebRequest.Create(Constants.FileFtpLink + galleryImage.FilePath);
				requestFileDelete.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUser"],
					ConfigurationManager.AppSettings["FTPPassword"]);
				requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
				using ((FtpWebResponse) requestFileDelete.GetResponse())
				{
					// Delete the GalleryImage
					_db.GalleryImages.Remove(galleryImage);
					_db.SaveChanges();

					return RedirectToAction("AdminGalleryImages", "GalleryImage");
				}
			}
			catch (Exception)
			{
				ModelState.AddModelError("", Resources.ErrorCodes.ERROR_LOGIN_FTP);
			}

			return View(galleryImage);
		}
	}
}