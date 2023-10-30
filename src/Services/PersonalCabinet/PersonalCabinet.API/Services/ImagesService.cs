using PersonalCabinet.API.Services.Interfaces;

using SkiaSharp;

namespace PersonalCabinet.API.Services;

public class ImagesService : IImagesService
{
	private const int Quality = 100;

	public byte[]? ConvertImageToByteArray(SKImage? image)
	{
		if (image is null)
			return null;

		return image.Encode(SKEncodedImageFormat.Jpeg, Quality).ToArray();
	}

	public SKImage? ResizeImage(SKImage? image, int targetWidth)
	{
		if (image is null)
			return null;

		SKImage resizedImage = null!;

		using (var original = SKBitmap.Decode(image.Encode().ToArray()))
		{
			double coefficient = (double)original.Width / targetWidth;

			var newPhotoWidth = Convert.ToInt32(original.Width / coefficient);
			var newPhotoHeight = Convert.ToInt32(original.Height / coefficient);

			var info = new SKImageInfo(newPhotoWidth, newPhotoHeight);

			using (var resized = original.Resize(info, SKFilterQuality.High))
			{
				if (resized == null)
					return null!;

				resizedImage = SKImage.FromBitmap(resized);
			}
		}

		return resizedImage;
	}

	public SKImage? ConvertByteArrayToImage(byte[]? image)
	{
		if (image is null)
			return null;

		using (var stream = new SKMemoryStream(image))
		{
			return SKImage.FromEncodedData(stream);
		}
	}

	public byte[] Resize(byte[]? image, int targetWidth)
	{
		using (var inputStream = new SKMemoryStream(image))
		{
			using (var codec = SKCodec.Create(inputStream))
			{
				using (var original = SKBitmap.Decode(codec))
				{
					double coefficient = (float)original.Width / targetWidth;

					var newPhotoWidth = Convert.ToInt32(original.Width / coefficient);
					var newPhotoHeight = Convert.ToInt32(original.Height / coefficient);

					if (newPhotoWidth > original.Width || newPhotoHeight > original.Height)
						return image!;

					using (SKBitmap resized = original.Resize(new SKImageInfo(newPhotoWidth, newPhotoHeight), SKFilterQuality.High))
					{
						var useWidth = resized.Width;
						var useHeight = resized.Height;

						Action<SKCanvas> transform = canvas => { };

						// иногда картинки переворачиваются при изменении размера, чтобы решить это добавлен следующий код
						switch (codec.EncodedOrigin)
						{
							case SKEncodedOrigin.TopLeft:
								break;
							case SKEncodedOrigin.TopRight:
								// flip along the x-axis
								transform = canvas => canvas.Scale(-1, 1, useWidth / 2, useHeight / 2);
								break;
							case SKEncodedOrigin.BottomRight:
								transform = canvas => canvas.RotateDegrees(180, useWidth / 2, useHeight / 2);
								break;
							case SKEncodedOrigin.BottomLeft:
								// flip along the y-axis
								transform = canvas => canvas.Scale(1, -1, useWidth / 2, useHeight / 2);
								break;
							case SKEncodedOrigin.LeftTop:
								useWidth = resized.Height;
								useHeight = resized.Width;
								transform = canvas =>
								{
									// Rotate 90
									canvas.RotateDegrees(90, useWidth / 2, useHeight / 2);
									canvas.Scale(useHeight * 1.0f / useWidth, -useWidth * 1.0f / useHeight, useWidth / 2, useHeight / 2);
								};
								break;
							case SKEncodedOrigin.RightTop:
								useWidth = resized.Height;
								useHeight = resized.Width;
								transform = canvas =>
								{
									// Rotate 90
									canvas.RotateDegrees(90, useWidth / 2, useHeight / 2);
									canvas.Scale(useHeight * 1.0f / useWidth, useWidth * 1.0f / useHeight, useWidth / 2, useHeight / 2);
								};
								break;
							case SKEncodedOrigin.RightBottom:
								useWidth = resized.Height;
								useHeight = resized.Width;
								transform = canvas =>
								{
									// Rotate 90
									canvas.RotateDegrees(90, useWidth / 2, useHeight / 2);
									canvas.Scale(-useHeight * 1.0f / useWidth, useWidth * 1.0f / useHeight, useWidth / 2, useHeight / 2);
								};
								break;
							case SKEncodedOrigin.LeftBottom:
								useWidth = resized.Height;
								useHeight = resized.Width;
								transform = canvas =>
								{
									// Rotate 90
									canvas.RotateDegrees(90, useWidth / 2, useHeight / 2);
									canvas.Scale(-useHeight * 1.0f / useWidth, -useWidth * 1.0f / useHeight, useWidth / 2, useHeight / 2);
								};
								break;
						}

						var info = new SKImageInfo(useWidth, useHeight);
						using (var surface = SKSurface.Create(info))
						{
							using (var paint = new SKPaint())
							{
								// high quality with antialiasing
								paint.IsAntialias = true;
								paint.FilterQuality = SKFilterQuality.High;

								// rotate according to origin
								transform.Invoke(surface.Canvas);

								// draw the bitmap to fill the surface
								surface.Canvas.DrawBitmap(resized, info.Rect, paint);
								surface.Canvas.Flush();

								using (SKImage sKimage = surface.Snapshot())
								{
									return sKimage.Encode(SKEncodedImageFormat.Jpeg, Quality).ToArray();
								}
							}
						}
					}
				}
			}
		}
	}
}