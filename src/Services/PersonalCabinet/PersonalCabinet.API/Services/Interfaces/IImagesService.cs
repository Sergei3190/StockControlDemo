using SkiaSharp;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Кроссплатформенный сервис по работе с изображениями
/// </summary>
public interface IImagesService
{
	/// <summary>
	/// Преобразование типа данных из Image в byte[]
	/// </summary>
	byte[]? ConvertImageToByteArray(SKImage? image);

	/// <summary>
	/// Изменение размера фотографии, загруженной из БД или другого источника, НО не из UI пользователя
	/// </summary>
	SKImage? ResizeImage(SKImage? image, int targetWidth);

	/// <summary>
	/// Преобразование типа данных из byte[] в Image
	/// </summary>
	SKImage? ConvertByteArrayToImage(byte[]? image);


	/// <summary>
	/// Изменение размера фотографии, загруженной из UI пользователя
	/// </summary>
	byte[] Resize(byte[]? image, int targetWidth);
}