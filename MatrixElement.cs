using System.ComponentModel;
using System.Runtime.CompilerServices;
using Matrix = Matrices.Matrix;


namespace Macierze_v2
{
    public class MatrixElement : INotifyPropertyChanged
    {
        private Matrix m;
        private string s;
        private char l;
        private string det;

        public event PropertyChangedEventHandler PropertyChanged;

        public MatrixElement() { }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string S
        {
            get
            {
                return s;
            }

            set
            {
                s = value;
                NotifyPropertyChanged();
            }
        }

        public Matrix M
        {
            get
            {
                return m;
            }

            set
            {
                m = value;
                NotifyPropertyChanged();
            }
        }

        public char L
        {
            get
            {
                return l;
            }

            set
            {
                l = value;
                NotifyPropertyChanged();
            }
        }

        public string Det
        {
            get
            {
                return det;
            }

            set
            {
                det = value;
                NotifyPropertyChanged();
            }
        }

    }
}