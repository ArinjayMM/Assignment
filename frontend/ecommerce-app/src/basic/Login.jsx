import axios from 'axios'
import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'

function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const navigate = useNavigate()

  const onLogin = async () => {
    if (email.length === 0) {
      toast.warn('Enter email address')
    } else if (password.length === 0) {
      toast.warn('Enter password')
    } else {
      try {
        const result = await axios.get(
          `https://localhost:7253/api/Users/${email}`,
          {
            params: { Password: password },
            Email: email,
            Password: password,
          }
        )
        console.log('Email : ', email)
        console.log('Password : ', password)
        if (result.status === 200) {
          toast.success('Login successful.')
          sessionStorage.setItem('userID', result.data.id)
          console.log('User ID : ',result.data.id)
          if (result.data.type === 'admin') {
            navigate('/admin-home')
          } else {
            navigate('/home')
          }
        }
      } catch (error) {
        toast.error('Wrong Email or Password.')
      }
    }
  }

  return (
    <>
      <div className='container'>
        <div className='row justify-content-center mt-5'>
          <div className='col-md-6'>
            <div className='card shadow'>
              <div className='card-body'>
                <h1 className='text-center'>Signin</h1>
                <div className='mb-3'>
                  <label htmlFor=''>Email</label>
                  <input
                    type='email'
                    onChange={(e) => setEmail(e.target.value)}
                    className='form-control'
                  />
                </div>
                <div className='mb-3'>
                  <label htmlFor=''>Password</label>
                  <input
                    type='password'
                    onChange={(e) => setPassword(e.target.value)}
                    className='form-control'
                  />
                </div>
                <div className='mb-3'>
                  <div className='mb-2 mt-4'>
                    Don't have an account? Register{' '}
                    <Link to='/register'>Here</Link>
                  </div>
                  <div className='btn-Login d-flex justify-content-start mt-3'>
                    <button
                      className='btn btn-primary'
                      style={{ marginRight: '2vw' }}
                      onClick={onLogin}
                    >
                      Sign in
                    </button>
                    <button type='button' className='btn btn-warning'>
                      <Link
                        to='/'
                        style={{ textDecoration: 'none', color: 'white' }}
                      >
                        <div className='signUp'>Cancel</div>
                      </Link>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}

export default Login
