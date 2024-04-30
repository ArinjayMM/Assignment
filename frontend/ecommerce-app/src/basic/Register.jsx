import axios from 'axios'
import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import validator from 'validator'

function Register() {
  const [email, setEmail] = useState('')
  const [emailError, setEmailError] = useState('')
  const [firstname, setFirstName] = useState('')
  const [lastname, setLastName] = useState('')
  const [password, setPassword] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const navigate = useNavigate()

  const validateEmail = (e) => {
    var email = e.target.value

    if (validator.isEmail(email)) {
      setEmailError('Valid Email :)')
      setEmail(email)
    } else {
      setEmailError('Enter valid Email!')
    }
  }

  const validatePassword = (value) => {
    if (
      validator.isStrongPassword(value, {
        minLength: 8,
        minLowercase: 1,
        minUppercase: 1,
        minNumbers: 1,
        minSymbols: 1,
      })
    ) {
      setPassword(value)
      setErrorMessage('Strong Password')
    } else {
      setErrorMessage('Weak Password')
    }
  }

  async function onRegister(event) {
    event.preventDefault()

    if (email.length === 0) {
      toast.warn('Enter email address')
    } else if (password.length === 0) {
      toast.warn('Enter password')
    } else if (firstname.length === 0) {
      toast.warn('Enter first name')
    } else if (lastname.length === 0) {
      toast.warn('Enter last name')
    } else {
      try {
        const result = await axios.post('https://localhost:7253/api/Users', {
          FirstName: firstname,
          LastName: lastname,
          Email: email,
          Password: password,
        })
        if (result.status === 201) {
          toast.success('Successfully registered your account')
          console.log(result.data)
          navigate('/login')
        } else {
          toast.error('Registration failed')
        }
      } catch (error) {
        toast.error('Registration failed')
      }
    }
  }

  return (
    <>
      <div className='container'>
        <div className='row justify-content-center mt-5'>
          <div className='col-md-6'>
            <div className='card shadow mb-5'>
              <div className='card-body'>
                <h1 className='col text-center'>Register</h1>
                <div className='mb-3'>
                  <label htmlFor=''>Email</label>
                  <input
                    onChange={(e) => validateEmail(e)}
                    type='email'
                    className='form-control'
                  />
                  <br />
                  <span
                    style={{
                      fontWeight: 'bold',
                      color: emailError === 'Valid Email :)' ? 'green' : 'red',
                    }}
                  >
                    {emailError}
                  </span>
                </div>
                <div className='mb-3'>
                  <label htmlFor=''>Password</label>
                  <input
                    onChange={(e) => validatePassword(e.target.value)}
                    type='password'
                    className='form-control'
                  />
                  <br />
                  {errorMessage === '' ? null : (
                    <span
                      style={{
                        fontWeight: 'bold',
                        color:
                          errorMessage === 'Strong Password' ? 'green' : 'red',
                      }}
                    >
                      {errorMessage}
                    </span>
                  )}
                </div>
                <div className='mb-3'>
                  <label htmlFor=''>First Name</label>
                  <input
                    onChange={(e) => setFirstName(e.target.value)}
                    type='text'
                    className='form-control'
                  />
                </div>
                <br />
                <div className='mb-3'>
                  <label htmlFor=''>Last Name</label>
                  <input
                    onChange={(e) => setLastName(e.target.value)}
                    type='text'
                    className='form-control'
                  />
                </div>
                <br />
                <div className='mb-3'>
                  <div className='mb-2'>
                    Already have an account? Sign in{' '}
                    <Link to='/login'>Here</Link>
                  </div>
                  <div className='btn-register'>
                    <button
                      onClick={onRegister}
                      className='btn btn-primary'
                      style={{ marginRight: '2vw' }}
                    >
                      Register
                    </button>
                    <button type='button' className='btn btn-warning'>
                      <Link
                        style={{ textDecoration: 'none', color: 'white' }}
                        to='/login'
                      >
                        Cancel
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

export default Register
