export default class FileService {
    static downloadFile(blob: Blob, fileName) {
      const link = document.createElement('a');
  
      link.href = URL.createObjectURL(blob);
      link.download = fileName;
      link.click();
  
      URL.revokeObjectURL(link.href);
    }
  }
  