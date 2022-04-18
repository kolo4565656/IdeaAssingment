import { PayloadAction } from '@reduxjs/toolkit';
import userApi from 'api/userApi';
import jwt_decode from 'jwt-decode';
import { JWT, Login, Response, User } from 'models';
import { push } from 'redux-first-history';
import { call, delay, fork, put, take } from 'redux-saga/effects';
import { authActions } from './authSlice';

function* handleLogin(payload: Login) {
  try {
    yield delay(500);
    const result: JWT = yield call(userApi.login, payload);
    localStorage.setItem('access_token', result.token);

    const decoded: Response = jwt_decode(result.token);
    const user: User = {
      id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      userName: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      email: decoded.email,
      firstName: decoded.firstName,
      lastName: decoded.lastName,
    };
    localStorage.setItem('info', JSON.stringify(user));
    yield put(authActions.loginSuccess(user));

    // redirect
    yield put(push('/idea'));
  } catch (error: any) {
    yield put(authActions.logout());
    yield put(authActions.loginFailed(error));
  }
}

function* handleLogout() {
  yield delay(500);
  localStorage.removeItem('access_token');
  localStorage.removeItem('info');
  // redirect to login page
  yield put(push('/login'));
}

function* watchLoginFlow() {
  while (true) {
    const isLoggedIn = Boolean(localStorage.getItem('access_token'));

    if (!isLoggedIn) {
      const action: PayloadAction<Login> = yield take(authActions.login.type); //watcher
      yield fork(handleLogin, action.payload); //worker
    }

    yield take(authActions.logout.type);
    yield call(handleLogout);
    //we use call(blocking) instead of fork to make sure the logout operation is done
  }
}

export default function* authSaga() {
  yield fork(watchLoginFlow);
}
