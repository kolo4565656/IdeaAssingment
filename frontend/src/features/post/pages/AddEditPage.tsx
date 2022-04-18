import { ChevronLeft, AddCircle, VerifiedRounded } from '@mui/icons-material';
import { Box, Button, Chip, Divider, TextField, Typography } from '@mui/material';
import categoryApi from 'api/categoryApi';
import postApi from 'api/postApi';
import { PostAdd } from 'models';
import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import PostForm from '../components/PostForm';

export default function AddEditPage() {
  const navigate = useNavigate();
  const [mediaFiles, setMediaFiles] = useState<File[]>([]);
  const [attachmentFiles, setAttachmentFiles] = useState<File[]>([]);
  const [isAddCat, setIsAddCat] = useState(false);
  const [addCat, setAddCat] = useState('');
  const [addedCat, setAddedCat] = useState(false);

  const handleAddCat = async () => {
    const prev = addCat;
    setAddCat('loading...');
    await categoryApi.add([{ id: 0, name: prev }]).then(() => {
      setAddCat('');
      setIsAddCat(false);
      setAddedCat(!addedCat);
    });
  };

  const handleFormSubmit = async (formValues: PostAdd) => {
    const categoriesToProcess: number[] = (formValues.categoryIds as string).split(',').map((x) => {
      return parseInt(x);
    });
    let fd = new FormData();
    fd.append('name', formValues.name);
    fd.append('content', formValues.content);
    fd.append('description', formValues.description);
    categoriesToProcess.forEach((x) => {
      fd.append('categoryIds', x.toString());
    });
    mediaFiles.forEach((x) => {
      fd.append('mediaFiles', x);
    });
    attachmentFiles.forEach((x) => {
      fd.append('attachments', x);
    });

    await postApi.add(fd).then(() => {
      navigate('../');
      toast.success('Save post successfully!');
    });
  };

  const initialValues = {
    name: '',
    content: '',
    description: '',
    categoryIds: '',
    postImage: '',
  } as PostAdd;

  return (
    <div style={{ width: '100%' }}>
      <Link to="..">
        <Typography
          borderColor={'blue'}
          variant="caption"
          style={{ display: 'flex', alignItems: 'center', fontSize: '24px' }}
        >
          <ChevronLeft fontSize="medium" /> Go back
        </Typography>
      </Link>
      <Box style={{ display: 'flex', justifyContent: 'flex-end' }}>
        <div style={{ width: '100%' }}>
          <Typography textAlign={'center'} variant="h4">
            Add New Idea
          </Typography>
          <Box display={'flex'} style={{ justifyContent: 'center' }} mt={3}>
            <PostForm
              addedCat={addedCat}
              mediaFiles={mediaFiles}
              attachmentFiles={attachmentFiles}
              setAttachmentFile={setAttachmentFiles}
              setMediaFiles={setMediaFiles}
              initialValues={initialValues}
              onSubmit={handleFormSubmit}
            />
          </Box>
        </div>
        <Divider style={{ fontWeight: 'bold' }} orientation="vertical" flexItem>
          <Chip style={{ fontSize: '16px', maxWidth: '150px' }} label="Options" />
        </Divider>
        <div style={{ marginRight: '50px', minWidth: '250px' }}>
          <Typography
            style={{ fontSize: '15px', color: 'grey', fontStyle: 'italic' }}
            variant="h5"
            component="p"
          >
            Can't find category in selection?
          </Typography>
          <Button
            onClick={() => {
              isAddCat ? handleAddCat() : setIsAddCat(true);
            }}
            disabled={isAddCat && addCat == ''}
            variant="outlined"
            color={isAddCat ? 'secondary' : 'primary'}
            startIcon={isAddCat ? <VerifiedRounded /> : <AddCircle />}
          >
            {isAddCat ? 'Confirm add' : 'Add new category'}
          </Button>
          {isAddCat ? (
            <TextField
              disabled={addCat == 'loading...'}
              style={{ display: 'block', marginTop: '8px' }}
              label="Category name"
              value={addCat}
              onChange={(e) => setAddCat(e.target.value)}
            />
          ) : (
            ''
          )}
        </div>
      </Box>
    </div>
  );
}
