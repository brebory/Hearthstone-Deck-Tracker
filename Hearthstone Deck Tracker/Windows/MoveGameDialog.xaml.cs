﻿#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;

#endregion

namespace Hearthstone_Deck_Tracker
{
	/// <summary>
	/// Interaction logic for MoveGameDialog.xaml
	/// </summary>
	public partial class MoveGameDialog
	{
		public Deck SelectedDeck;
		public SerializableVersion SelectedVersion;

		public MoveGameDialog(IEnumerable<Deck> decks)
		{
			InitializeComponent();

			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			ListViewDecks.Items.Clear();
			foreach(var deck in decks.OrderByDescending(d => d.Name))
				ListViewDecks.Items.Add(new NewDeckPickerItem(deck));
		}

		private void ListViewDecks_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach(var item in e.AddedItems)
			{
				var pickerItem = item as NewDeckPickerItem;
				if(pickerItem == null)
					continue;
				DeckList.Instance.ActiveDeck = pickerItem.DataContext as Deck;
				pickerItem.OnSelected();
			}
			foreach(var item in e.RemovedItems)
			{
				var pickerItem = item as NewDeckPickerItem;
				if(pickerItem == null)
					continue;
				pickerItem.OnDelselected();
			}
			var dpi = ListViewDecks.SelectedItem as NewDeckPickerItem;
			if(dpi != null)
			{
				SelectedDeck = dpi.Deck;
				ComboBoxVersions.Items.Clear();
				foreach(var version in SelectedDeck.VersionsIncludingSelf)
					ComboBoxVersions.Items.Add(version);
				ComboBoxVersions.SelectedItem = SelectedDeck.SelectedVersion;
			}
		}

		private void ComboBoxVersions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedVersion = ComboBoxVersions.SelectedItem as SerializableVersion;
		}

		private void ButtonMoveToSelected_OnClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}