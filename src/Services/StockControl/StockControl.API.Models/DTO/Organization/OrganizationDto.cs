﻿using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.Interfaces;

namespace StockControl.API.Models.DTO.Organization;

/// <summary>
/// Организация
/// </summary>
public class OrganizationDto : NamedEntityDto, IClassifierItem
{
	public NamedEntityDto? Classifier { get; set; }

	// Здесь дополнимтельные поля справочника

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(Name)}: {Name}" +
		$"{nameof(Classifier)}: {Classifier?.Id} {Classifier?.Name}";
}