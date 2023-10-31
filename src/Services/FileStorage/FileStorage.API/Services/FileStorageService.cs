using FileStorage.API.Infrastructure.Settings;
using FileStorage.API.Models;
using FileStorage.API.Services.Interfaces;

using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.API.Services;

public class FileStorageService : IFileStorageService
{
	private readonly MongoDbSettings _mongoDbSettings;

	public FileStorageService(IOptionsSnapshot<MongoDbSettings> options)
	{
		_mongoDbSettings = options.Value;
	}

	public async Task<Guid> UnloadAsync(StorageFileInfo fileInfo, Stream content)
	{
		ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));
		ArgumentNullException.ThrowIfNull(content, nameof(content));

		var connect = _mongoDbSettings.Type.Equals(nameof(_mongoDbSettings.Local), StringComparison.OrdinalIgnoreCase)
			? _mongoDbSettings.Local
			: _mongoDbSettings.DockerNoSql;

		var client = new MongoClient(connect);
		var db = client.GetDatabase(_mongoDbSettings.Catalog);

		// для работы с большими файлами https://metanit.com/sharp/mongodb/1.17.php
		var bucket = new GridFSBucket(db);
		var objectId = await bucket.UploadFromStreamAsync(fileInfo.OriginalName, content).ConfigureAwait(false);
		fileInfo.FileContentId = objectId.ToString();

		var collection = db.GetCollection<StorageFileInfo>(_mongoDbSettings.DefaultCollection);
		fileInfo.Id = Guid.NewGuid();
		await collection.InsertOneAsync(fileInfo).ConfigureAwait(false);

		return fileInfo.Id;
	}

	public async Task<FileModel?> DownloadByIdAsync(Guid id)
	{
		var connect = _mongoDbSettings.Type.Equals(nameof(_mongoDbSettings.Local), StringComparison.OrdinalIgnoreCase)
			? _mongoDbSettings.Local
			: _mongoDbSettings.DockerNoSql;

		var client = new MongoClient(connect);
		var db = client.GetDatabase(_mongoDbSettings.Catalog);
		var collection = db.GetCollection<StorageFileInfo>(_mongoDbSettings.DefaultCollection);

		var cursor = await collection.FindAsync(x => x.Id == id).ConfigureAwait(false);
		var fileInfo = await cursor.SingleOrDefaultAsync().ConfigureAwait(false);

		if (fileInfo == null)
			return null;

		var bucket = new GridFSBucket(db);
		var content = await bucket.DownloadAsBytesAsync(ObjectId.Parse(fileInfo.FileContentId)).ConfigureAwait(false);

		var file = new FileModel { Info = fileInfo, Content = content };
		return file;
	}
}