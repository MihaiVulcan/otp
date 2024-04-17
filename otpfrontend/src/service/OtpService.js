import axios from 'axios';

const getOtp = async (email) => {
    console.log(email)

    return axios.post('/otp/getotp', {
        email: email,
      })
}

const checkOtp = async (email, code) => {
    console.log(email, code)

    axios.create({
        headers: [{"ValidationToker":""}]
    })

    return axios.post('/otp/checkotp', {
        email: email,
        code: code
    })
} 

export {getOtp, checkOtp}