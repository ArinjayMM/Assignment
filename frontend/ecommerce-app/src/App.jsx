import React from 'react'
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import './App.css'
import Cart from './user/components/Cart'
import Home from './user/components/Home'
import Login from './basic/Login'
import Order from './user/components/Order'
import Register from './basic/Register'
import FrontPage from './basic/FrontPage'
import AdminHome from './admin/components/AdminHome'
import AdminCart from './admin/components/AdminCart'
import AdminOrder from './admin/components/AdminOrder'

function App() {
  return (
    <Router>
      <div>
        <ToastContainer />
        <Routes>
          <Route path='/' element={<FrontPage />} />
          <Route path='/login' element={<Login />} />
          <Route path='/register' element={<Register />} />
          <Route path='/home' element={<Home />} />
          <Route path='/cart' element={<Cart />} />
          <Route path='/order' element={<Order />} />
          <Route path='/admin-home' element={<AdminHome/>}/>
          <Route path='/admin-cart' element={<AdminCart/>}/>
          <Route path='/admin-order' element={<AdminOrder/>}/>
        </Routes>
      </div>
    </Router>
  )
}

export default App
