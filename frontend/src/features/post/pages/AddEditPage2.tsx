import { AddCircle, ChevronLeft, VerifiedRounded, Edit } from '@mui/icons-material';
import { Box, Button, Chip, Divider, TextField, Typography } from '@mui/material';
import categoryApi from 'api/categoryApi';
import postApi from 'api/postApi';
import { ChangePostImg } from 'components/FormFields/ChangePostImg';
import { PostAdd } from 'models';
import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { toast } from 'react-toastify';
import PostForm from '../components/PostForm';

export default function AddEditPage2() {
  const navigate = useNavigate();
  const { postId } = useParams<{ postId: string }>();
  const [post, SetPost] = useState<PostAdd>();

  useEffect(() => {
    (async () => {
      try {
        if (postId) {
          const data: PostAdd = await postApi.getForAddEdit(postId);
          SetPost(data);
        }
      } catch (error) {
        console.log('Failed to fetch post details', error);
      }
    })();
  }, [postId]);

  const initialValues = {
    name: '',
    content: '',
    description: '',
    ...post,
  } as PostAdd;

  const handleFormSubmit = async (formValues: PostAdd) => {
    console.log(formValues);
    await postApi
      .update(
        { name: formValues.name, description: formValues.description, content: formValues.content },
        postId
      )
      .then(() => {
        navigate('../');
        toast.success('Save post successfully!');
      });
  };

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
      <Box style={{ display: 'flex', justifyContent: 'center' }}>
        <div style={{ width: '100%' }}>
          <Typography textAlign={'center'} variant="h4">
            Update Idea
          </Typography>

          <Box display={'flex'} style={{ justifyContent: 'center' }} mt={3}>
            <PostForm isEdit={true} initialValues={initialValues} onSubmit={handleFormSubmit} />
          </Box>
        </div>
        <Divider style={{ fontWeight: 'bold' }} orientation="vertical" flexItem>
          <Chip style={{ fontSize: '16px' }} label="Options" />
        </Divider>
        <ChangePostImg postId={postId} />
      </Box>
    </div>
  );
}
