﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Lisa.Quartets.Mobile
{
	public partial class ConfirmationView : ContentPage
	{
        public ConfirmationView(Card card)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
            _selectedCard = card;
            SetSelectedCard();
		}

		public async void YesClicked(object sender, EventArgs args)
		{ 
            SaveCard();

            if (_database.IsQuartet(_selectedCard))
            {
                _database.SetQuartet(_selectedCard.Category);
                Navigation.RemovePage(this);
                await Navigation.PushAsync(new QuartetView(_selectedCard.Category));
            }
            else
            {
                Navigation.RemovePage(this);
                await Navigation.PushAsync(new RequestView());
            }
		}

		public async void NoClicked(object sender, EventArgs args)
		{
			Navigation.RemovePage(this);
			await Navigation.PushAsync(new IdleView());
        }

        public void SetSelectedCard()
        {
            CardImage card = new CardImage();
            TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                if (!_soundPlaying)
                {
                    PlaySound();
                }
            };

            card.GestureRecognizers.Add(gestureRecognizer);
            card.Source = _selectedCard.FileName;
            layout.Children.Add(card);
        }

        private void SaveCard()
        {
            _selectedCard.IsInHand = 1;
            _database.CreateOrUpdateCard(_selectedCard);
        }

        public async void PlaySound() 
        {
            _soundPlaying = true;
            DependencyService.Get<IAudio>().PlayFile(_selectedCard.SoundFile);
            await Task.Delay(2000);
            _soundPlaying = false;
        }

        private bool _soundPlaying = false;

        private Card _selectedCard;
        private CardDatabase _database = new CardDatabase();
	}
}