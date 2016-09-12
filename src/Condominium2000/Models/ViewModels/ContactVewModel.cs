using Condominium2000.Helpers;
using Condominium2000.Models.Context;

namespace Condominium2000.Models.ViewModels
{
	public class ContactViewModel
	{
		public enum FormType
		{
			None,
			Error,
			Board,
			IT
		}

		public ContactViewModel()
		{
		}

		public ContactViewModel(ref Condominium2000Context context, FormType type)
		{
			// Get Template
			ContactTemplate = TemplateHelper.GetContactTemplate(ref context);

			// Forms
			PopulateContactForm(type);
		}

		public ContactTemplate ContactTemplate { get; set; }

		public ErrorForm ErrorForm { get; set; }
		public BoardForm BoardForm { get; set; }
		public ItForm ItForm { get; set; }

		public void PopulateContactForm(FormType type)
		{
			switch (type)
			{
				case FormType.Error:
				{
					ErrorForm = new ErrorForm();
					// Convert ErrorFormCategories to a list

					ErrorForm.CategoryList = ErrorForm.GetSelectList();
					break;
				}
				case FormType.Board:
				{
					BoardForm = new BoardForm();
					// Convert BoardFormCategories to a list

					BoardForm.CategoryList = BoardForm.GetSelectList();
					break;
				}
				case FormType.IT:
				{
					ItForm = new ItForm();
					// Convert ITFormCategories to a list

					ItForm.CategoryList = ItForm.GetSelectList();
					break;
				}
				case FormType.None:
				default:
				{
					break;
				}
			}
		}
	}
}