import { Edit } from '@mui/icons-material';
import { Button, Typography } from '@mui/material';
import postApi from 'api/postApi';
import React, { useState } from 'react';

export interface ChangePostImgProps {
  postId: string;
}

export function ChangePostImg({ postId }: ChangePostImgProps) {
  const [value, setValue] = useState<File>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setValue(new File([e.target.files[0]], 'interface.png', { type: 'image/png' }));
  };

  const handleClick = async () => {
    let fd = new FormData();
    fd.append('files', value);
    await postApi.postMediaFile(fd, postId).then(() => {
      setValue(null);
    });
  };

  return (
    <div style={{ marginRight: '50px' }}>
      <Typography
        style={{ fontSize: '15px', color: 'grey', fontStyle: 'italic' }}
        variant="h5"
        component="p"
      >
        Change post signature image? Click here!
      </Typography>
      <Button style={{ padding: '0' }} onClick={() => {}} variant="outlined" color={'primary'}>
        <label style={{ cursor: 'pointer', padding: '6px 12px' }} htmlFor="file">
          <Edit style={{ marginBottom: '-5px', fontSize: '20px' }} />
          &nbsp;{value ? value.name : 'Change post image'}
        </label>
        <input
          onChange={(e) => handleChange(e)}
          type="file"
          name="file"
          id="file"
          className="inputfile"
          multiple
        ></input>
      </Button>
      {value ? (
        <Button
          onClick={() => handleClick()}
          style={{ display: 'block', marginTop: '5px' }}
          variant="contained"
        >
          Save
        </Button>
      ) : (
        ''
      )}
    </div>
  );
}
