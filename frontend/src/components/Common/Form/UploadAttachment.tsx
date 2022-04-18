//@ts-nocheck
import { Upload, message } from 'antd';
import { InboxOutlined } from '@ant-design/icons';

const { Dragger } = Upload;

interface UploadAttachmentProps {
  attachmentFiles?: File[];
  setAttachmentFile?: React.Dispatch<React.SetStateAction<File[]>>;
}

export default function UploadAttachment(param: UploadAttachmentProps) {
  const { attachmentFiles, setAttachmentFile } = param;
  const props = {
    name: 'file',
    multiple: true,
    beforeUpload: () => {
      return false;
    },
    onChange(info) {
      const { status } = info.file;
      if (status !== 'uploading') {
        setAttachmentFile(
          info.fileList.map((e) => {
            return e.originFileObj;
          })
        );
      }
    },
  };
  return (
    <div style={{ width: '100%' }}>
      <Dragger
        {...props}
        accept=".png, .jpg, .jpeg, .txt, .mp4, .mp3, .gif, .pdf, .xlsx, .docx, .csv"
      >
        <p className="ant-upload-drag-icon">
          <InboxOutlined />
        </p>
        <p className="ant-upload-text">Click or drag file to this area to upload ATTACHMENT</p>
      </Dragger>
    </div>
  );
}
