namespace Plugins.IngameDebugConsole.Scripts
{
	public class DebugLogIndexList<T>
	{
		T[] indices;
		int size;

		public int Count => size;

		public T this[int index]
		{
			get => indices[index];
			set => indices[index] = value;
		}

		public DebugLogIndexList()
		{
			indices = new T[64];
			size = 0;
		}

		public void Add( T value )
		{
			if( size == indices.Length )
				System.Array.Resize( ref indices, size * 2 );

			indices[size++] = value;
		}

		public void Clear()
		{
			size = 0;
		}

		public int IndexOf( T value )
		{
			return System.Array.IndexOf( indices, value );
		}
	}
}