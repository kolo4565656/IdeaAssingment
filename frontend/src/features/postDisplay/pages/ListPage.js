import { AddCircle, Close, RemoveCircle, Search, ThumbDown, ThumbUp } from '@mui/icons-material';
import {
  Autocomplete,
  Box,
  Button,
  Chip,
  Divider,
  FormControl,
  IconButton,
  InputLabel,
  Modal,
  OutlinedInput,
  Rating,
  TextField,
  Tooltip,
} from '@mui/material';
import Typography from '@mui/material/Typography';
import { Card, Col, Row } from 'antd';
import 'antd/dist/antd.css';
import categoryApi from 'api/categoryApi';
import postApi from 'api/postApi';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import '../../../assets/styles/main.css';
import '../../../assets/styles/responsive.css';
import { TreeSelect } from 'antd';
const { TreeNode } = TreeSelect;

const convertDate = (date) => {
  const dateParts = date.split('T');
  return dateParts[0];
};

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

function ListPage() {
  const [categoryOption, setCategoryOption] = React.useState([]);
  const [deletee, setDelete] = useState('');
  const [open, setOpen] = React.useState(false);
  const handleOpen = (id) => {
    setDelete(id);
    setOpen(true);
  };
  const handleClose = () => {
    setDelete('');
    setOpen(false);
  };
  const handleDelete = () => {
    (async () => {
      await postApi.delete(deletee).then(() => {
        handleClose();
        setParams({ ...params });
      });
    })();
  };

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
    pageIndex: 0,
    pageSize: 7,
    sort: '',
    keyword: '',
    category: -1,
  });

  const [search, setsSarch] = useState('');
  const [searchCat, setSearchCat] = useState({ id: -1, label: '' });

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
    <div>
      <Card bordered={false} className="header-solid mb-24">
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
                setParams({ ...params, category: -1, keyword: '', sort: '' });
                setsSarch('');
                setSearchCat('');
              }}
            >
              <Close style={{ fontSize: '18px', marginBottom: '-4px' }} />
              Clear
            </Button>
          </div>
        </div>
        <b>Sorting: </b>
        <TreeSelect
          value={params.sort}
          placeholder="Sorting"
          listHeight={800}
          style={{ width: 300, marginBottom: '24px' }}
          treeDefaultExpandAll
          onSelect={(e, value) => setParams({ ...params, sort: value.value })}
        >
          <TreeNode style={{ pointerEvents: 'none' }} title="Recent">
            <TreeNode value="lastmodified" title="Ascending"></TreeNode>
            <TreeNode value="lastmodified DESC" title="Descending"></TreeNode>
          </TreeNode>
          <TreeNode style={{ pointerEvents: 'none' }} title="Comments">
            <TreeNode value="commentcount" title="Ascending"></TreeNode>
            <TreeNode value="commentcount DESC" title="Descending"></TreeNode>
          </TreeNode>
          <TreeNode style={{ pointerEvents: 'none' }} title="Rating">
            <TreeNode value="(totalstar/commentcount)" title="Ascending"></TreeNode>
            <TreeNode value="(totalstar/commentcount) DESC" title="Descending"></TreeNode>
          </TreeNode>
          <TreeNode style={{ pointerEvents: 'none' }} title="Like">
            <TreeNode value="fake1" title="Ascending"></TreeNode>
            <TreeNode value="fake2" title="Descending"></TreeNode>
          </TreeNode>
        </TreeSelect>
        <Row gutter={[24, 24]}>
          {Array.isArray(post.data) &&
            post.data.map((p, index) => (
              <Col span={24} md={12} xl={6} key={index}>
                <Divider />
                <Card
                  style={{ position: 'relative' }}
                  bordered={false}
                  className="card-project"
                  cover={<img src={`/Upload/${p.id}/Media/interface.png`} />}
                >
                  <div className="card-tag">{p.titlesub}</div>
                  <h5>{p.name}</h5>
                  <p style={{ fontStyle: 'italic', marginBottom: '12px' }}>{p.description} </p>
                  <p>Creator: {p.creatorName}</p>
                  <p>Last Update: {convertDate(p.lastModified)}</p>
                  <Typography style={{ display: 'flex', width: '100%' }}>
                    <div style={{ fontWeight: 'bold', marginRight: '5px' }}>Rating:</div>
                    <Rating
                      name="read-only"
                      value={p.totalStar / p.commentCount}
                      readOnly
                      precision={0.5}
                    />
                    <div style={{ marginLeft: '5px' }}>({p.commentCount})</div>
                  </Typography>
                  <Typography
                    style={{
                      marginTop: '6px',
                      marginRight: '6px',
                      pointerEvents: 'none',
                      display: 'flex',
                      alignItems: 'center',
                      position: 'absolute',
                      top: '0',
                      right: '0',
                    }}
                  >
                    <Button
                      style={{
                        border: '1px solid white',
                        borderRadius: '25%/50%',
                        marginRight: '15px',
                        backgroundColor: '#00cc99',
                      }}
                      variant="contained"
                      startIcon={<ThumbUp style={{ marginTop: '-7px', color: 'white' }} />}
                    >
                      {p.likeCount}
                    </Button>
                    <Button
                      style={{
                        borderRadius: '25%/50%',
                        backgroundColor: '#990000',
                        border: '1px solid white',
                      }}
                      variant="contained"
                      startIcon={<ThumbDown color="error" style={{ color: 'white' }} />}
                    >
                      {p.dislikeCount}
                    </Button>
                  </Typography>
                  <Row className="card-footer">
                    <Col style={{ width: '100%' }}>
                      <Link to={`${p.id}`} style={{ textDecoration: 'none', width: '90%' }}>
                        <Button style={{ backgroundColor: 'green', color: 'white', width: '100%' }}>
                          VIEW IDEA
                        </Button>
                      </Link>
                    </Col>
                    {currentUser.role == 'Admin' ? (
                      <Col style={{ width: '100%' }}>
                        <Button
                          onClick={() => handleOpen(p.id)}
                          style={{
                            backgroundColor: 'red',
                            color: 'white',
                            width: '100%',
                            marginTop: '12px',
                          }}
                        >
                          DELETE as ADMIN
                        </Button>
                      </Col>
                    ) : (
                      ''
                    )}
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
      {post.data.length == 0 ? (
        <div style={{ display: 'flex', justifyContent: 'center' }}>
          <img src="/pngtree-empty-box-icon-for-your-project-png-image_1521417.jpg" />
        </div>
      ) : (
        ''
      )}
    </div>
  );
}

export default ListPage;
