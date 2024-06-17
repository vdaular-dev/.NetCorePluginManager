﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ProductDataTriggers.cs
 *
 *  Purpose:  Triggers for product table
 *
 *  Date        Name                Reason
 *  29/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables.Products
{
	internal class ProductDataTriggers : ITableTriggers<ProductDataRow>
	{
		private readonly ISimpleDBOperations<StockDataRow> _stockData;
		private const int MinimumDescriptionLength = 20;
		private const int MaximumDescriptionLength = 3000;
		private const int MinimumNameLength = 5;
		private const int MaximumNameLength = 100;
		private const int MinimumSkuLength = 3;

		public ProductDataTriggers(ISimpleDBOperations<StockDataRow> stockData)
		{
			_stockData = stockData ?? throw new ArgumentNullException(nameof(stockData));
		}

		public int Position => 0;

		public TriggerType TriggerTypes => TriggerType.BeforeUpdate | TriggerType.BeforeInsert | TriggerType.AfterInsert | TriggerType.BeforeDelete;

		public void AfterDelete(List<ProductDataRow> records)
		{
			// from interface but unused in this context
		}

		public void AfterInsert(List<ProductDataRow> records)
		{
			IReadOnlyList<StockDataRow> allStock = _stockData.Select();
			List<StockDataRow> stockData = new();

			records.ForEach(r =>
			{
				if (!allStock.Any(sd => sd.ProductId.Equals(r.Id)))
					stockData.Add(new StockDataRow() { ProductId = r.Id });
			});

			_stockData.Insert(stockData);
		}

		public void AfterUpdate(List<ProductDataRow> records)
		{
			// from interface but unused in this context
		}

		public void BeforeDelete(List<ProductDataRow> records)
		{
			records.ForEach(r =>
			{
				List<StockDataRow> stockData = _stockData.Select(sd => sd.ProductId.Equals(r.Id)).ToList();

				if (stockData.Count > 0)
					_stockData.Delete(stockData);
			});
		}

		public void BeforeInsert(List<ProductDataRow> records)
		{
			records.ForEach(r => ValidateData(r));
		}

		public void BeforeUpdate(List<ProductDataRow> records)
		{
			records.ForEach(r => ValidateData(r));
		}

		public void BeforeUpdate(ProductDataRow newRecord, ProductDataRow oldRecord)
		{
			// from interface but unused in this context
		}

		private void ValidateData(ProductDataRow row)
		{
			if (String.IsNullOrEmpty(row.Name))
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Name), "Can not be null or empty");

			if (row.Name.Length < MinimumNameLength)
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Name), $"Minimum length for {nameof(row.Name)} is {MinimumNameLength} characters");

			if (row.Name.Length > MaximumNameLength)
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Name), $"Maximum length for {nameof(row.Name)} is {MaximumNameLength} characters");

			if (String.IsNullOrEmpty(row.Description))
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Description), "Can not be null or empty");

			if (row.Description.Length < MinimumDescriptionLength)
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Description), $"Minimum length for {nameof(row.Description)} is {MinimumDescriptionLength} characters");

			if (row.Description.Length > MaximumDescriptionLength)
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Description), $"Maximum length for {nameof(row.Description)} is {MaximumDescriptionLength} characters");

			if (String.IsNullOrEmpty(row.Sku) || row.Sku.Length < MinimumSkuLength)
				throw new InvalidDataRowException(nameof(ProductDataRow), nameof(row.Sku), $"Minimum length for {nameof(row.Sku)} is {MinimumSkuLength} characters");
		}
	}
}
