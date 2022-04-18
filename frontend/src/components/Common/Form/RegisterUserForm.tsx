import { yupResolver } from '@hookform/resolvers/yup';
import { Box, Button, CircularProgress, Modal, Typography } from '@mui/material';
import { InputField, SelectField, SelectOption } from 'components/FormFields';
import { FieldInfo, Role, User } from 'models';
import { useForm, UseFormGetValues } from 'react-hook-form';
import * as yup from 'yup';
import React, { useEffect, useState } from 'react';
import userApi from 'api/userApi';
import EditIcon from '@mui/icons-material/Edit';
import ChangePassword from './ChangePassword';

const validationSchema = yup.object({
  userName: yup.string().required('Username is required'),
  email: yup.string().required('Email is required'),
  firstName: yup.string().required('First name is required'),
  lastName: yup.string().required('Last Name is required'),
  role: yup.string().nullable(),
});

export interface RegisterUserFormProps {
  userId?: string;
  onSubmit?: (formValues: User, dirtyFields: any[]) => void;
}

export default function RegisterUserForm({ onSubmit, userId }: RegisterUserFormProps) {
  useEffect(() => {
    if (userId) {
      (async () => {
        try {
          const user: User = await userApi.getById(userId);
          reset(user);
        } catch (error) {
          console.log('Failed to fetch user details', error);
        }
      })();
    } else {
      reset({
        userName: '',
        email: '',
        firstName: '',
        lastName: '',
        role: -1,
      });
    }
  }, [userId]);
  const {
    control,
    handleSubmit,
    setValue,
    getValues,
    register,
    reset,
    formState: { isSubmitting, isDirty, isValid },
    formState,
  } = useForm<User>({
    defaultValues: {
      userName: '',
      email: '',
      firstName: '',
      lastName: '',
      role: -1,
    },
    resolver: yupResolver(validationSchema),
  });

  const handleFormSubmit = async (formValues: User) => {
    try {
      const dirtyFields = Object.keys(formState.dirtyFields).map((x) => ({
        field: x,
        value: getValues(x as 'userName' | 'email' | 'firstName' | 'lastName' | 'role' | 'id'),
      }));
      await onSubmit?.(formValues, dirtyFields);
    } catch (error: any) {
      console.log(error);
    }
  };

  const roleOptions: SelectOption[] = [
    { label: 'admin', value: Role.admin },
    { label: 'staff', value: Role.staff },
  ];

  return (
    <>
      <Box
        maxWidth={800}
        style={{
          backgroundColor: 'white',
          marginTop: '72px',
          marginBottom: '72px',
          padding: '24px',
        }}
      >
        <Typography textAlign={'center'} variant="h4">
          {userId ? 'Edit Infomation' : 'Add New User'}
        </Typography>
        <form onSubmit={handleSubmit(handleFormSubmit)}>
          <InputField name="userName" control={control} label="User Name" />
          <InputField name="email" control={control} label="Email" />
          <InputField name="firstName" control={control} label="First Name" />
          <InputField name="lastName" control={control} label="Last Name" />
          {!userId && (
            <SelectField name="role" options={roleOptions} control={control} label="Role" />
          )}
          <Box mt={3}>
            <Button
              fullWidth
              type="submit"
              variant="contained"
              color="primary"
              disabled={isSubmitting || !isDirty}
            >
              {isSubmitting && <CircularProgress size={16} color="primary" />}
              &nbsp;Save
            </Button>
          </Box>
        </form>
        {userId ? <ChangePassword userId={userId} /> : ''}
      </Box>
    </>
  );
}
