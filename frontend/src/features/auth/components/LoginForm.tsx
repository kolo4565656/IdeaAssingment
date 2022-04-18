import { yupResolver } from '@hookform/resolvers/yup';
import { Alert, Box, Button, CircularProgress } from '@mui/material';
import { useAppSelector } from 'app/hooks';
import { InputField } from 'components/FormFields';
import { Login } from 'models';
import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import { selectIsLogging } from '../authSlice';

export interface LoginFormProps {
  initialValues?: Login;
  Failed?: boolean;
  onSubmit?: (formValues: Login) => void;
}

const schema = yup.object().shape({
  userName: yup
    .string()
    .required('Please enter username')
    .min(4, 'Please enter at least 4 characters'),
  password: yup
    .string()
    .required('Please enter password')
    .max(10, 'Please enter under 10 characters'),
});

export default function LoginForm({ initialValues, onSubmit, Failed }: LoginFormProps) {
  const [error, setError] = useState<string>('');
  const isLogging = useAppSelector(selectIsLogging);

  const {
    control,
    handleSubmit,
    formState: { isSubmitting },
  } = useForm<Login>({
    defaultValues: initialValues,
    resolver: yupResolver(schema),
  });

  useEffect(() => {
    if (Failed) {
      setError('Username or password is wrong');
    }
  }, [Failed]);

  const handleFormSubmit = (formValues: Login) => {
    try {
      // Clear previous submission error
      setError('');

      onSubmit?.(formValues);
    } catch (error: any) {
      setError(error.message);
    }
  };

  return (
    <Box maxWidth={400}>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <InputField name="userName" control={control} label="Email" />
        <InputField name="password" control={control} label="Password" type={'password'} />
        {error && <Alert severity="error">{error}</Alert>}

        <Box mt={3}>
          <Button
            fullWidth
            type="submit"
            variant="contained"
            color="secondary"
            disabled={isLogging}
          >
            {isLogging && <CircularProgress size={20} color="primary" />}
            &nbsp;Login
          </Button>
        </Box>
      </form>
    </Box>
  );
}
