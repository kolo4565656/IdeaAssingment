import { TextField } from '@mui/material';
import * as React from 'react';
import { InputHTMLAttributes } from 'react';
import { Control, useController } from 'react-hook-form';

export interface InputFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  name: string;
  control: Control<any>;
  label?: string;
  disableEffect?: boolean;
  subOnchange?: (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void;
}

export function InputField({
  name,
  control,
  label,
  disableEffect,
  subOnchange,
  ...inputProps
}: InputFieldProps) {
  const {
    field: { value, onChange, onBlur, ref },
    fieldState: { invalid, error },
  } = useController({
    name,
    control,
  });

  return (
    <TextField
      InputLabelProps={disableEffect ? { shrink: false } : {}}
      fullWidth
      size="small"
      margin="normal"
      value={value}
      onChange={(e) => {
        onChange(e);
        subOnchange && subOnchange(e);
      }}
      onBlur={onBlur}
      label={label}
      variant="outlined"
      inputRef={ref}
      error={invalid}
      helperText={error?.message}
      inputProps={inputProps}
    />
  );
}
