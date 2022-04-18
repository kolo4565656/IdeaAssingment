import { Edit, Cancel, Save } from '@mui/icons-material';
import { Button, TextField } from '@mui/material';
import userApi from 'api/userApi';
import React, { useState } from 'react';

interface ChangePasswordProps {
  userId?: string;
}

export default function ChangePassword(props: ChangePasswordProps) {
  const { userId } = props;
  const [oldPass, setOldPass] = useState('password');
  const [newPass, setNewPass] = useState('password');
  const [toggle, setToggle] = useState(true);
  const [error, setError] = useState(false);

  const Unsavable = () => {
    return (
      newPass == 'password' ||
      newPass == '' ||
      oldPass == 'password' ||
      oldPass == '' ||
      newPass.length < 8 ||
      oldPass.length < 8
    );
  };

  const HandleRest = () => {
    setError(false);
    setToggle(true);
    setNewPass('password');
    setOldPass('password');
  };

  return (
    <div style={{ width: '100%' }}>
      <Button
        onClick={async () => {
          if (Unsavable()) {
            setToggle(!toggle);
          } else {
            await userApi
              .changePassword({
                userId: userId,
                newPassword: newPass,
                currentPassword: oldPass,
              })
              .then((e) => {
                if (e) {
                  HandleRest();
                } else {
                  setError(true);
                }
              });
          }
        }}
        style={{ margin: '12px 0', width: '35%', marginLeft: '65%', fontSize: '20px' }}
        variant="outlined"
        color="primary"
        startIcon={toggle ? <Edit /> : Unsavable() ? <Cancel /> : <Save />}
      >
        {toggle ? 'Change password' : Unsavable() ? 'Cancel' : 'Save'}
      </Button>
      {toggle ? (
        ''
      ) : (
        <div>
          <div
            style={{
              width: '100%',
              textAlign: 'right',
              color: 'red',
              margin: '8px 0',
            }}
          >
            {' '}
            {error ? 'Password is not matched!' : ''}
          </div>
          <i style={{ color: 'gray', marginLeft: '50%', textAlign: 'right' }}>
            (Password's length must equal or greater than 8)
          </i>
          <TextField
            type={'password'}
            value={oldPass}
            onFocus={() => {
              oldPass == 'password' && setOldPass('');
            }}
            onChange={(e) => setOldPass(e.target.value)}
            fullWidth
            style={{ display: 'block', width: '50%', marginLeft: '50%', marginBottom: '8px' }}
            size="small"
            label="Current password"
            variant="outlined"
          />
          <TextField
            type={'password'}
            value={newPass}
            onFocus={() => {
              newPass == 'password' && setNewPass('');
            }}
            onChange={(e) => setNewPass(e.target.value)}
            fullWidth
            style={{ display: 'block', width: '50%', marginLeft: '50%' }}
            size="small"
            label="New password"
            variant="outlined"
          />
        </div>
      )}
    </div>
  );
}
