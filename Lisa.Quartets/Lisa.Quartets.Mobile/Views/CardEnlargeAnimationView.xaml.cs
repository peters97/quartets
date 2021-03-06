﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Lisa.Quartets.Mobile
{
	public partial class CardEnlargeAnimationView : ContentPage
	{
		public CardEnlargeAnimationView()
		{
			InitializeComponent();
			EnsureCardsExist();
			SetImages();
		}

		private void EnsureCardsExist()
		{
			var cards = _database.RetrieveCards();
			if (cards.Count() == 0)
			{
				_database.CreateDefaultCards();
			}
		}

		private void OnImageClick(object sender, EventArgs args)
		{	
			var image = (Image) sender;

			if (IsSelected(image))
			{
				Deselect(image);
				image.FadeTo(0.5, 100);
				image.ScaleTo(0.8, 100);
			}
			else
			{
				Select(image);
				image.FadeTo(1, 100);
				image.ScaleTo(1, 100);
			}
		}

		private bool IsSelected(Image image)
		{
			return _selectedImages.Contains(image.ClassId);
		}

		private void Select(Image image)
		{
			_selectedImages.Add(image.ClassId);
		}

		private void Deselect(Image image)
		{
			_selectedImages.Remove(image.ClassId);
		}

		private void SetImages()
		{
			var cards = _database.RetrieveCards();

			foreach (var card in cards) 
			{
				System.Diagnostics.Debug.WriteLine(card.FileName);
			}
		}

		private CardDatabase _database = new CardDatabase();
		private List<string> _selectedImages = new List<string>();
	}
}

