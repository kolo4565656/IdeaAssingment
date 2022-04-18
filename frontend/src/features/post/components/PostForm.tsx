// @ts-nocheck
import { yupResolver } from '@hookform/resolvers/yup';
import { Alert, Box, Button, CircularProgress, InputLabel } from '@mui/material';
import { Checkbox } from 'antd';
import UploadAttachment from 'components/Common/Form/UploadAttachment';
import { InputField, SelectField, SelectOption } from 'components/FormFields';
import AdvanceMultipleSelect from 'components/FormFields/AdvanceMultipleSelect';
import { TextAreaField } from 'components/FormFields/TextAreaField';
import { ApiResponse, Category, ListParams, PostAdd } from 'models';
import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import categoryApi from '../../../api/categoryApi';

export interface PostFormProps {
  addedCat?: boolean;
  isEdit?: boolean;
  initialValues?: PostAdd;
  mediaFiles?: File[];
  setMediaFiles?: React.Dispatch<React.SetStateAction<File[]>>;
  attachmentFiles?: File[];
  setAttachmentFile?: React.Dispatch<React.SetStateAction<File[]>>;
  onSubmit?: (formValues: PostAdd) => void;
}

const initParams: ListParams = {
  keyword: '',
  pageIndex: 0,
  pageSize: 1000,
  sort: '',
};

export default function PostForm({
  isEdit = false,
  initialValues,
  onSubmit,
  addedCat,
  setMediaFiles,
  mediaFiles,
  attachmentFiles,
  setAttachmentFile,
}: PostFormProps) {
  const contraintAll = {
    name: yup.string().required('Please enter name.'),
    content: yup.string().required('Please enter content.'),
    description: yup.string().required('Please enter description.'),
  };

  const schema = isEdit
    ? yup.object().shape(contraintAll)
    : yup.object().shape({
        ...contraintAll,
        categoryIds: yup.string().required('Please enter category.'),
        postImage: yup.string().required('Select an image for your post!'),
      });

  const [check, setCheck] = useState(false);
  const [error, setError] = useState<string>('');
  const [categoryParams, SetCategoryParams] = useState<ListParams>(initParams);
  const [options, SetOptions] = useState<Category[]>([]);

  const subOnchange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    if (isEdit) return;
    const file: File | null = (e.target as HTMLInputElement).files[0];
    const newfile = new File([file], 'interface.png', { type: 'image/png' });
    setMediaFiles([...mediaFiles, newfile]);
  };

  useEffect(() => {
    if (isEdit) return;
    (async () => {
      try {
        const data: ApiResponse<Category> = await categoryApi.get(categoryParams);
        if (Array.isArray(data.data)) {
          SetOptions(data.data);
        }
      } catch (error) {
        console.log('Failed to fetch categories', error);
      }
    })();
  }, [addedCat]);

  const {
    control,
    handleSubmit,
    setValue,
    register,
    getValues,
    reset,
    formState: { isSubmitting, isDirty, isValid },
  } = useForm<PostAdd>({
    defaultValues: initialValues,
    resolver: yupResolver(schema),
  });

  useEffect(() => {
    if (isEdit) {
      reset(initialValues);
    }
  }, [initialValues]);

  const handleFormSubmit = async (formValues: PostAdd) => {
    try {
      // Clear previous submission error
      setError('');

      await onSubmit?.(formValues);
    } catch (error: any) {
      console.log(error);
      setError(error.message);
    }
  };

  return (
    <Box style={{ width: '100%' }}>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <InputField name="name" control={control} label="Post Name" />
        <TextAreaField name="content" control={control} label="Content" />
        <InputField name="description" control={control} label="Description" />
        {isEdit ? (
          ''
        ) : (
          <>
            <AdvanceMultipleSelect
              option={options}
              name="categoryIds"
              setValue={setValue}
              register={register}
              control={control}
              label="Categories -- select"
            />
            <InputLabel style={{ marginBottom: '-15px', marginTop: '15px' }}>Post Image</InputLabel>
            <InputField
              label=""
              disableEffect
              name="postImage"
              control={control}
              type={'file'}
              style={{ marginTop: 0 }}
              subOnchange={subOnchange}
              accept="image/png, image/jpeg"
            />
          </>
        )}
        {isEdit ? (
          ''
        ) : (
          <UploadAttachment
            attachmentFiles={attachmentFiles}
            setAttachmentFile={setAttachmentFile}
          />
        )}

        {error && <Alert severity="error">{error}</Alert>}
        {isEdit ? (
          ''
        ) : (
          <Checkbox checked={check} onChange={(e) => setCheck(e.target.checked)}>
            Agree with{' '}
            <a
              target="_blank"
              style={{ textDecoration: 'underline' }}
              href={'https://www.epicgames.com/site/en-US/tos?lang=en-US'}
            >
              terms and condition
            </a>
          </Checkbox>
        )}
        <Box mt={3}>
          <Button
            fullWidth
            type="submit"
            variant="contained"
            color="primary"
            disabled={isSubmitting || !isDirty || !check}
          >
            {isSubmitting && <CircularProgress size={16} color="primary" />}
            &nbsp;Save
          </Button>
        </Box>
      </form>
    </Box>
  );
}
