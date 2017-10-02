using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.CommonCore
{
	public class RefillableList<T>
	{
		public RefillableList(FillListDelegate fillListDelegate)
		{
			_fillListDelegate = fillListDelegate;
		}

		public T Next()
		{
			if(_currentQueue == null || !_currentQueue.Any())
			{
				_currentQueue = new Queue<T>(_fillListDelegate(_fillIteration++));
			}

			return _currentQueue.Dequeue();
		}

		public delegate IEnumerable<T> FillListDelegate(int timesListHasBeenRefilled);

		private readonly FillListDelegate _fillListDelegate;
		private Queue<T> _currentQueue;

		private int _fillIteration = 1;
	}
}
