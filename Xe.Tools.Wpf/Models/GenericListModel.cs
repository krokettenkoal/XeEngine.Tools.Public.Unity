using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Wpf.Models
{
	public class GenericListModel<T> : BaseNotifyPropertyChanged, IEnumerable, IEnumerable<T>
	{
		private T selectedItem;
		private int selectedIndex;
		protected readonly ObservableCollection<T> list;

		public GenericListModel(IEnumerable<T> list)
		{
			Items = this.list = list != null ?
				new ObservableCollection<T>(list) :
				new ObservableCollection<T>();

			AddCommand = new RelayCommand(x =>
			{
				this.list.Add(OnNewItem());
				OnPropertyChanged(nameof(Items));
			}, x => this.list != null);

			RemoveCommand = new RelayCommand(x =>
			{
				var index = Items.IndexOf(SelectedItem);
				this.list.RemoveAt(index);
			}, x => SelectedItem != null && SelectedItem is T);

			MoveUpCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(--selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => SelectedItem != null && SelectedItem is T);

			MoveDownCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(++selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => SelectedItem != null && SelectedItem is T);
		}

		public ObservableCollection<T> Items { get; private set; }
        public ObservableCollection<T> UnfilteredItems => list;

		public T SelectedItem
		{
			get => selectedItem;
			set
			{
				selectedItem = value;
				OnSelectedItem(value);
				NotifyItemSelected();
			}
		}

		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				selectedIndex = value;
				NotifyItemSelected();
			}
		}

		public bool IsItemSelected => selectedItem != null;

		public RelayCommand AddCommand { get; }

		public RelayCommand RemoveCommand { get; }

		public RelayCommand MoveUpCommand { get; }

		public RelayCommand MoveDownCommand { get; }

		private void NotifyItemSelected()
		{
			RemoveCommand.CanExecute(SelectedIndex);
			MoveUpCommand.CanExecute(SelectedIndex);
			MoveDownCommand.CanExecute(SelectedIndex);

			OnPropertyChanged(nameof(SelectedItem));
			OnPropertyChanged(nameof(SelectedIndex));
			OnPropertyChanged(nameof(IsItemSelected));
			OnPropertyChanged(nameof(RemoveCommand));
			OnPropertyChanged(nameof(MoveUpCommand));
			OnPropertyChanged(nameof(MoveDownCommand));
        }

        public IEnumerator GetEnumerator() => Items.GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();

        public void Filter(Func<T, bool> selector = null)
		{
			Items = selector != null ?
				new ObservableCollection<T>(list.Where(selector)) : list;
			OnPropertyChanged(nameof(Items));
		}

		public void Filter(Func<IEnumerable<T>, IEnumerable<T>> selector)
		{
			Items = new ObservableCollection<T>(selector(list));
			OnPropertyChanged(nameof(Items));
		}

        protected virtual T OnNewItem() => throw new NotImplementedException();

		protected virtual void OnSelectedItem(T item)
		{

		}
    }
}
