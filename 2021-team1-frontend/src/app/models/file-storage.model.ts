export interface FileStorage {
  fileName: string;
  relativePath: string;
  fullPath: string;
  contentType: string;
  uri: URL;
  dateModified: string | Date;
}
