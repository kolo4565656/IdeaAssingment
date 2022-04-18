import { VerticalAlignTopOutlined } from '@ant-design/icons';
import {
  Autocomplete,
  Button,
  Chip,
  Divider,
  FormControl,
  IconButton,
  InputLabel,
  OutlinedInput,
  TextField,
  Tooltip,
} from '@mui/material';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Typography from '@mui/material/Typography';
import { Avatar, Card, Col, Descriptions, Row, Switch, Upload } from 'antd';
import { RemoveCircle, AddCircle, Search, Close } from '@mui/icons-material';
import 'antd/dist/antd.css';
import postApi from 'api/postApi';
import userApi from 'api/userApi';
import RegisterUserForm from 'components/Common/Form/RegisterUserForm';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import BgProfile from '../../../assets/images/bg-profile.jpg';
import profilavatar from '../../../assets/images/face-1.jpg';
import '../../../assets/styles/main.css';
import '../../../assets/styles/responsive.css';
import categoryApi from 'api/categoryApi';

const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  p: 4,
};

const convertDate = (date) => {
  const dateParts = date.split('T');
  return dateParts[0];
};

function ListPage() {
  const [categoryOption, setCategoryOption] = React.useState([]);
  const [open, setOpen] = React.useState(false);
  const handleOpen = (id) => {
    setDelete(id);
    setOpen(true);
  };
  const handleClose = () => {
    setDelete('');
    setOpen(false);
  };
  const [deletee, setDelete] = useState('');

  const [openModal, setModal] = useState(false);
  const [post, setPost] = useState({
    paging: {
      pageIndex: 0,
      pageSize: 7,
      totalItemsCount: 0,
      totalPages: 0,
    },
    data: [],
  });
  const [params, setParams] = useState({
    isSelfSearch: true,
    pageIndex: 0,
    pageSize: 7,
    sort: '',
    keyword: '',
    category: -1,
  });

  const [search, setsSarch] = useState('');
  const [searchCat, setSearchCat] = useState({ id: -1, label: '' });

  const handleDelete = () => {
    (async () => {
      await postApi.delete(deletee).then(() => {
        handleClose();
        setParams({ ...params });
      });
    })();
  };

  const info = localStorage.getItem('info');
  let currentUser = {
    lastName: '',
    firstName: '',
    userName: '',
    email: '',
    id: '',
    role: '',
  };
  if (info) {
    currentUser = JSON.parse(info);
  }

  const closeModal = () => {
    setModal(false);
  };

  const registerUser = async (formValues, dirtyFields) => {
    await userApi
      .edit(
        dirtyFields.reduce((x, y) => {
          return {
            ...x,
            [`${y.field}`]: `${y.value}`,
          };
        }, {}),
        currentUser.id
      )
      .then(() => {
        closeModal();
      });
  };

  useEffect(() => {
    (async () => setPost(await postApi.get(params)))();
  }, [params]);

  useEffect(() => {
    (async () => {
      const data = (
        await categoryApi.get({
          keyword: '',
          pageIndex: 0,
          pageSize: 1000,
          sort: '',
        })
      ).data;
      setCategoryOption(
        data.map((x) => {
          return {
            id: x.id,
            label: x.name,
          };
        })
      );
    })();
  }, []);

  return (
    <div style={{ marginTop: '100px' }}>
      <div className="profile-nav-bg" style={{ backgroundImage: 'url(' + BgProfile + ')' }}></div>

      <Card
        className="card-profile-head"
        bodyStyle={{ display: 'none' }}
        title={
          <Row justify="space-between" align="middle" gutter={[24, 0]}>
            <Col span={24} md={12} className="col-info">
              <Avatar.Group>
                <Avatar size={74} shape="square" src={profilavatar} />

                <div className="avatar-info">
                  <h4 className="font-semibold m-0">{`${currentUser?.firstName} ${currentUser?.lastName}`}</h4>
                  <p>{currentUser.role == 'Admin' ? 'Administrator' : 'Idea Creator'}</p>
                </div>
              </Avatar.Group>
            </Col>
          </Row>
        }
      ></Card>

      <Row gutter={[24, 0]} style={{ justifyContent: 'center' }}>
        <Col span={24} md={10} className="mb-24 ">
          <Card
            bordered={false}
            className="header-solid h-full"
            title={<h6 className="font-semibold m-0">Platform Settings</h6>}
          >
            <ul className="list settings-list">
              <li>
                <h6 className="list-header text-sm text-muted">COMMUNICATION</h6>
              </li>
              <li>
                <Switch defaultChecked />
                <span>Email me when someone comments on my post</span>
              </li>
              <li>
                <Switch />
                <span>Email me when someone answers my comment</span>
              </li>
            </ul>
          </Card>
        </Col>
        <Col span={24} md={10} className="mb-24">
          <Card
            bordered={false}
            title={<h6 className="font-semibold m-0">Profile Information</h6>}
            className="header-solid h-full card-profile-information"
          >
            <Descriptions style={{ paddingLeft: '24' }}>
              <Descriptions.Item label="Full Name" span={3}>
                {`${currentUser?.firstName} ${currentUser?.lastName}`}
              </Descriptions.Item>
              <Descriptions.Item label="Role" span={3}>
                {`${currentUser?.role}`}
              </Descriptions.Item>
              <Descriptions.Item label="Email" span={3}>
                {`${currentUser?.email}`}
              </Descriptions.Item>
              <Descriptions.Item>
                <Button onClick={() => setModal(true)} className="showmore" type="link">
                  SHOW MORE
                </Button>
              </Descriptions.Item>
            </Descriptions>
          </Card>
        </Col>
      </Row>
      <Card
        bordered={false}
        className="header-solid mb-24"
        title={
          <>
            <h6 className="font-semibold">My Idea</h6>
            <p>Idea design houses</p>
          </>
        }
      >
        <div style={{ display: 'flex', alignItems: 'center' }}>
          <FormControl
            style={{ width: '500px', marginBottom: '8px' }}
            variant="outlined"
            size="small"
          >
            <InputLabel style={{}} htmlFor="searchByName">
              Search by name
            </InputLabel>
            <OutlinedInput
              id="searchByName"
              style={{ width: '500px' }}
              label="Search by name"
              endAdornment={<Search />}
              onChange={(e) => setsSarch(e.target.value)}
              value={search}
            />
          </FormControl>
          <Button
            onClick={() => setParams({ ...params, keyword: search })}
            style={{
              display: 'inline-block',
              border: '1px solid black',
              marginLeft: '5px',
              maxHeight: '40px',
              marginBottom: '8px',
            }}
          >
            Submit
          </Button>
          <div style={{ display: 'inline-block', marginBottom: '8px', marginLeft: 'auto' }}>
            <Autocomplete
              value={searchCat}
              id="combo-box-demo"
              size="small"
              style={{ display: 'inline-block', marginBottom: '13px' }}
              options={categoryOption}
              disableClearable
              onChange={(event, value) => {
                console.log('call');
                setSearchCat(value);
                value && setParams({ ...params, category: value.id });
              }}
              sx={{ width: 300 }}
              renderInput={(params) => <TextField {...params} value={searchCat} label="Category" />}
            />
            <Button
              variant="contained"
              color="error"
              style={{
                display: 'inline-block',
                height: '40px',
                width: '95px',
                marginLeft: '5px',
                marginTop: '16px',
                marginBottom: '0',
              }}
              onClick={() => {
                setParams({ ...params, category: -1, keyword: '' });
                setsSarch('');
                setSearchCat('');
              }}
            >
              <Close style={{ fontSize: '18px', marginBottom: '-4px' }} />
              Clear
            </Button>
          </div>
        </div>
        <Row gutter={[24, 24]}>
          <Col span={24} md={12} xl={6}>
            <Link to="add" style={{ textDecoration: 'none', width: '90%' }}>
              <Upload
                disabled
                style={{ cursor: 'pointer' }}
                listType="picture-card"
                className="avatar-uploader projects-uploader"
              >
                <div className="ant-upload-text font-semibold text-dark">
                  {<VerticalAlignTopOutlined style={{ width: 20, color: '#000' }} />}
                  <Button style={{ display: 'block', fontSize: '18px' }} type="link">
                    Add New Idea
                  </Button>
                </div>
              </Upload>
            </Link>
          </Col>
          {Array.isArray(post.data) &&
            post.data.map((p, index) => (
              <Col span={24} md={12} xl={6} key={index}>
                <Divider />
                <Card
                  bordered={false}
                  className="card-project"
                  cover={<img src={`/Upload/${p.id}/Media/interface.png`} />}
                >
                  <div className="card-tag">{p.titlesub}</div>
                  <h5>{p.name}</h5>
                  <p style={{ fontStyle: 'italic', marginBottom: '12px' }}>{p.description} </p>
                  <p>Creator: {p.creatorName}</p>
                  <p>Last Update: {convertDate(p.lastModified)}</p>
                  <Row gutter={[6, 0]} className="card-footer">
                    <Col span={12}>
                      <Link to={`${p.id}`} style={{ textDecoration: 'none', width: '90%' }}>
                        <Button style={{ backgroundColor: 'green', color: 'white' }}>
                          VIEW IDEA
                        </Button>
                      </Link>
                    </Col>
                    <Col style={{ display: 'flex', justifyContent: 'right' }} span={12}>
                      <Button
                        onClick={() => handleOpen(p.id)}
                        style={{ backgroundColor: 'red', color: 'white' }}
                      >
                        DELETE
                      </Button>
                    </Col>
                  </Row>
                  <hr />
                  <b>Categories:</b>
                  {Array.isArray(p.categories) &&
                    p.categories.map((x) => (
                      <Chip
                        style={{ marginLeft: '5px', marginRight: '5px', marginBottom: '5px' }}
                        label={`${x.name}`}
                      />
                    ))}
                </Card>
              </Col>
            ))}
        </Row>
        <Divider light style={{ marginTop: '16px', marginBottom: '16px' }} />
        <div
          style={{
            border: '1px solid grey',
            display: 'inline-block',
            borderRadius: '25px',
            float: 'right',
          }}
        >
          <Tooltip title="Show more">
            <IconButton
              disabled={params.pageSize >= post.paging.totalItemsCount}
              onClick={() => setParams({ ...params, pageSize: params.pageSize + 8 })}
              size="medium"
            >
              <AddCircle style={{ fontSize: '48px', color: 'green' }} />
            </IconButton>
          </Tooltip>
          <Tooltip title="Show less">
            <IconButton
              disabled={params.pageSize <= 7}
              onClick={() => setParams({ ...params, pageSize: params.pageSize - 8 })}
              size="medium"
            >
              <RemoveCircle style={{ fontSize: '48px', color: 'red' }} />
            </IconButton>
          </Tooltip>
        </div>
      </Card>
      <Modal
        open={openModal}
        onClose={closeModal}
        style={{
          display: 'flex',
          justifyContent: 'center',
        }}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <>
          <Button
            style={{
              marginTop: '72px',
              marginBottom: '72px',
            }}
            type="submit"
            variant="contained"
            color="secondary"
            onClick={closeModal}
          >
            &nbsp;Back
          </Button>
          <RegisterUserForm userId={currentUser.id} onSubmit={registerUser} />
        </>
      </Modal>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Confirm?
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            Do you want to process delete?
          </Typography>
          <Button
            fullWidth
            onClick={handleDelete}
            style={{ backgroundColor: '#00CED1', color: 'black', marginTop: '24px' }}
          >
            YES
          </Button>
        </Box>
      </Modal>
    </div>
  );
}

export default ListPage;
