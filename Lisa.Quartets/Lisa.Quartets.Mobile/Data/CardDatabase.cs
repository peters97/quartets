﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace Lisa.Quartets.Mobile
{
	public class CardDatabase
	{
		public CardDatabase()
		{
			_database = DependencyService.Get<ISQLite>().GetConnection();
			_database.CreateTable<Card>();
		}

		public void DeleteCards()
		{
			_database.DeleteAll<Card>();
		}

		public List<Card> RetrieveCards()
		{
			return _database.GetAllWithChildren<Card>();
		}

		public int CreateOrUpdateCard(Card card)
		{
			if (card.Id != 0)
			{
				_database.InsertOrReplaceWithChildren(card);
				return card.Id;
			}
			else
			{
				return _database.Insert(card);
			}
		}

		public int DeleteCard(int id)
		{
			return _database.Delete<Card>(id);
		}

		public void CreateCard(Card card)
		{
			_database.InsertWithChildren(card);
		}

		public Card RetrieveCard(int cardId)
		{
			return _database.GetWithChildren<Card>(cardId);
		}

        public void ResetCards()
        {
            _database.Execute("UPDATE Card SET IsInHand = 0");
        }

        public void UpdateSelectedCards(List<int> ids)
        {
            ResetCards();

            string idString = string.Join(",", ids.Select(n => n.ToString()).ToArray());
			// REVIEW: Why call string.Format and then still use string concatenation?
            string query = string.Format("UPDATE 'Card' SET IsInHand = 1 WHERE Id in (" + idString + ")");

            _database.Execute(query);
        }

        public void GiveCardAway(int id)
        {
            _database.Execute("UPDATE Card SET IsInHand = 0 WHERE Id = ?", id);
        }

		// REVIEW: What's the purpose of the inHand parameter? Shouldn't RetrieveCardsInHand always return the cards in hand?
		public List<Card> RetrieveCardsInHand(int inHand)
        {
			return _database.Query<Card>("SELECT * FROM Card WHERE IsInHand =" + inHand);
        }

		public void CreateDefaultCards()			
		{
			for (int i = 1; i <= 44; i++)
			{
				var card = new Card();
				card.Name = string.Format("card{0}", i);
				card.Category = "test";
				card.FileName = string.Format("card{0}.png", i);
				card.IsInHand = 0;

				CreateCard(card);
			}
		}

		private SQLiteConnection _database;
	}
}