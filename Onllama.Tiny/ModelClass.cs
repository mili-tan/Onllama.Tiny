using AntdUI;

namespace Onllama.Tiny
{
    public class ModelsClass : NotifyProperty
    {
        string _name;

        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        string _size;

        public string size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged("size");
            }
        }

        DateTime? _modifiedAt;

        public DateTime? modifiedAt
        {
            get => _modifiedAt;
            set
            {
                _modifiedAt = value;
                OnPropertyChanged("modifiedAt");
            }
        }

        CellTag[]? _families;

        public CellTag[]? families
        {
            get => _families;
            set
            {
                _families = value;
                OnPropertyChanged("families");
            }
        }

        CellTag[]? _status;

        public CellTag[]? status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        CellTag[]? _quantization;

        public CellTag[]? quantization
        {
            get => _quantization;
            set
            {
                _quantization = value;
                OnPropertyChanged("quantization");
            }
        }

        CellLink[]? _btns;

        public CellLink[]? btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged("btns");
            }
        }
    }

}
