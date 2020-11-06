export class FileDownloader {
	public static DownloadFile(path: string, fileName: string) {
		fetch(path)
			.then(resp => resp.blob())
			.then(blob => {
				const url = window.URL.createObjectURL(blob);
				const a = document.createElement('a');
				a.style.display = 'none';
				a.href = url;
				a.download = fileName;
				document.body.appendChild(a);
				a.click();
				window.URL.revokeObjectURL(url);
			});
	}
}
