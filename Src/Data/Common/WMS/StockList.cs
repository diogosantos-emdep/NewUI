using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
	//[nsatpute][08.09.2025][GEOS2-9210]
    public class StockList
    {
        private int _idarticle;
        private string _reference;
        private string _description;
        private int _stock;
        private double _price;
        private double _value;
        private int _minimumStock;
        private int _maximumStock;
        private string _location;
        private int _quantity;
        private double _accumulated;
        private double _unitPrice;
        private int _idOtItem;
        private int _idArticlesStock;
        private int _regularisationPoint;
        private string _typeofmaterial;

        public StockList
            (int idarticle, string reference,
            string description, int stock, double price,
            double value, int minimumStock, int maximumStock,
            string location, double accumulated, double unitPrice,
            int quantity, int idOtItem, int idArticlesStock, int regularisationPoint, string typeofmaterial)
        {
            _idarticle = idarticle;
            _reference = reference;
            _description = description;
            _stock = stock;
            _price = price;
            _value = value;
            _minimumStock = minimumStock;
            _maximumStock = maximumStock;
            _location = location;
            _quantity = quantity;
            _accumulated = accumulated;
            _unitPrice = unitPrice;
            _idOtItem = idOtItem;
            _idArticlesStock = idArticlesStock;
            RegularisationPoint = regularisationPoint;
            _typeofmaterial = typeofmaterial;
        }

        public StockList
            (int idArticle, int quantity,
            double unitPrice,
            double price, int idOtItem, double accumulated, int idArticlesStock)
        {
            _idarticle = idArticle;
            _quantity = quantity;
            _unitPrice = unitPrice;
            _price = price;
            _idOtItem = idOtItem;
            _accumulated = accumulated;
            _idArticlesStock = idArticlesStock;
        }


        public string TypeofMaterial
        {
            get { return _typeofmaterial; }
            set { _typeofmaterial = value; }
        }
        public int IdArticle
        {
            get { return _idarticle; }
            set { _idarticle = value; }
        }

        public string Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int Stock
        {
            get { return _stock; }
            set { _stock = value; }
        }

        public int MinimumStock
        {
            get { return _minimumStock; }
            set { _minimumStock = value; }
        }

        public int MaximumStock
        {
            get { return _maximumStock; }
            set { _maximumStock = value; }
        }

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public double Accumulated
        {
            get { return _accumulated; }
            set { _accumulated = value; }
        }

        public double UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }

        public int IdOtItem
        {
            get { return _idOtItem; }
            set { _idOtItem = value; }
        }

        public int IdArticlesStock
        {
            get { return _idArticlesStock; }
            set { _idArticlesStock = value; }
        }

        public int RegularisationPoint
        {
            get { return _regularisationPoint; }
            set { _regularisationPoint = value; }
        }
        public double Discount { get; set; }
        private double exchangeRate;


    }
}
