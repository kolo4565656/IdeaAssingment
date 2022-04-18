import { Category, PostAdd } from 'models';
import Multiselect from 'multiselect-react-dropdown';
import * as React from 'react';
import { Control, useController, UseFormRegister, UseFormSetValue } from 'react-hook-form';

export interface InputFieldProps extends React.InputHTMLAttributes<HTMLInputElement> {
  name: string;
  control: Control<any>;
  label?: string;
  option?: Category[];
  register: UseFormRegister<PostAdd>;
  setValue: UseFormSetValue<PostAdd>;
}

export default function AdvanceMultipleSelect({
  name,
  control,
  label,
  option,
  register,
  setValue,
  ...inputProps
}: InputFieldProps) {
  const {
    field: { value, onChange, onBlur, ref },
    fieldState: { invalid, error },
  } = useController({
    name,
    control,
  });
  const [categoryName, setCategoryName] = React.useState<string>('');
  const [clean, setClean] = React.useState<boolean>(true);

  React.useEffect(() => {
    if (clean) {
      setClean(false);
    } else {
      setValue('categoryIds', categoryName, {
        shouldValidate: true,
        shouldDirty: true,
        shouldTouch: true,
      });
    }
  }, [categoryName]);

  const handleSelect = (e: Category[]) => {
    setCategoryName(
      e
        .map((x) => {
          return x.id;
        })
        .join(',')
    );
  };

  return (
    <div>
      <Multiselect
        style={
          invalid
            ? {
                searchBox: {
                  border: '1px solid red',
                  marginTop: '5px',
                },
                multiselectContainer: {
                  color: 'black',
                },
              }
            : {
                searchBox: {
                  marginTop: '14px',
                },
                multiselectContainer: {
                  color: 'black',
                },
              }
        }
        placeholder={label}
        isObject={true}
        displayValue="name"
        onRemove={handleSelect}
        onSelect={handleSelect}
        avoidHighlightFirstOption
        options={option}
        selectionLimit={4}
      />
      {invalid && (
        <span
          style={{
            color: '#d32f2f',
            fontFamily: '"Roboto","Helvetica","Arial",sans-serif',
            fontSize: '0.77rem',
            marginRight: '14px',
            marginLeft: '14px',
          }}
          className="invalid"
        >
          {error?.message}
        </span>
      )}
      <input
        style={{ display: 'none' }}
        {...register('categoryIds')}
        onBlur={onBlur}
        ref={ref}
        name={name}
        value={value}
        onChange={onChange}
      />
    </div>
  );
}
